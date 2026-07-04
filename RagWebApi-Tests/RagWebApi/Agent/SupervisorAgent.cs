using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Mistral.SDK;
using Newtonsoft.Json;
using RagWebApi.Agent.WorkerAgents;

namespace RagWebApi.Agent
{

    public class RouteDecision
    {
        public string? TaskInstructions { get; set; }
        public string? EmailInstructions { get; set; }
    }

    public interface ISupervisorAgent
    {
        Task<RagAgentResponse> ProcessRequestAsync(string input, string sessionId);
    }

    public class SupervisorAgent(IChatClient client, TaskAgent taskWorker, EmailAgent emailWorker, AgentSessionManager sessionManager) : ISupervisorAgent
    {
        private readonly AIAgent _taskBot = taskWorker.Create();
        private readonly AIAgent _emailBot = emailWorker.Create();

        public async Task<RagAgentResponse> ProcessRequestAsync(string input, string sessionId)
        {

            try
            {
                // Step 1: split intent deterministically via structured output
                var routeOptions = new ChatOptions
                {
                    ModelId = ModelDefinitions.MistralLarge,
                    Temperature = 0f,
                    ResponseFormat = ChatResponseFormat.ForJsonSchema<RouteDecision>()
                };


                var agentOption = new ChatClientAgentRunOptions
                {
                    ChatOptions = routeOptions
                };

                var routeMessages = new List<ChatMessage>
        {
            new(ChatRole.System, """
                Extract task-related instructions and/or email-related instructions from the user's request.
                TaskInstructions: non-null only if part of the request is about adding/listing/completing/deleting tasks.
                EmailInstructions: non-null only if part of the request is about composing/sending email.
                Both may be non-null if the request contains both. If neither applies, both are null.
                Output the response in a string prompt that can be parsed as JSON with the following schema:
                {
                    "TaskInstructions": string or null,
                    "EmailInstructions": string or null
                }
                """),
            new(ChatRole.User, input)
        };

                var routeResponse = await client.GetResponseAsync(routeMessages, routeOptions);
                var route = JsonConvert.DeserializeObject<RouteDecision>(routeResponse.Text)!;

                var parts = new List<RagAgentResponse>();
                bool taskCompleted = false;

                // Step 2: explicit, guaranteed dispatch — not left to the model's discretion
                if (!string.IsNullOrWhiteSpace(route.TaskInstructions))
                {
                    AgentSession session = await sessionManager.GetOrCreateSessionAsync(sessionId, _taskBot);
                    var r = await _taskBot.RunAsync(new ChatMessage(ChatRole.User, route.TaskInstructions), session, agentOption);
                    var response = JsonConvert.DeserializeObject<RagAgentResponse>(r.Text)!;
                    taskCompleted = response.Completed;
                    parts.Add(response);
                }

                if ((string.IsNullOrWhiteSpace(route.TaskInstructions) || taskCompleted) && !string.IsNullOrWhiteSpace(route.EmailInstructions))
                {
                    if (taskCompleted)
                        await Task.Delay(2000);
                    AgentSession session = await sessionManager.GetOrCreateSessionAsync(sessionId, _emailBot);
                    var r = await _emailBot.RunAsync(new ChatMessage(ChatRole.User, route.EmailInstructions), session, agentOption);
                    parts.Add(JsonConvert.DeserializeObject<RagAgentResponse>(r.Text)!);
                }

                if (parts.Count == 0)
                    return new RagAgentResponse { Success = false, Message = "Request didn't match task or email capabilities." };

                // Step 3: combine deterministically
                return new RagAgentResponse
                {
                    Success = parts.All(p => p.Success),
                    Message = string.Join(" ", parts.Select(p => p.Message)),
                    Data = parts.FirstOrDefault(p => p.Data is not null)?.Data ?? []
                };
            }
            catch (Exception e)
            {
                return RagAgentResponse.ErrorResponse(e.Message);
            }
        }
    }
}