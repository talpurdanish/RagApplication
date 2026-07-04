
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Mistral.SDK;

namespace RagWebApi.Agent.WorkerAgents

{

    public class RouterAgent(IChatClient client)
    {



        public AIAgent Create() {

            var chatOptions = new ChatOptions
            {
                ModelId = ModelDefinitions.MistralLarge,
                Temperature = 0.1f, // Low temperature ensures accurate routing decisions
                ResponseFormat = Microsoft.Extensions.AI.ChatResponseFormat.ForJsonSchema<RagAgentResponse>(),
                Instructions = """
                You are a routing assistant. Analyze the user's intent.
                Respond with exactly one of these words:
                - TASK : If the user wants to add, list, complete, or delete tasks.
                - EMAIL: If the user wants to compose, write, or send an email.
                wait for the reply from child agents and return to the user in a single response.
                Return Format for all commands should contains
                    1. success : True or false
                    2. message : string message about the task
                Always confirm what you did in plain language, not just raw tool output.
                Do not include any extra punctuation or conversational filler.
                """,


            };


            var routerAgentOptions = new ChatClientAgentOptions
            {
                Name = "RouterBot",
                ChatOptions = chatOptions
            };

            var triageAgent = client.AsAIAgent(
                options: routerAgentOptions

            );

            return triageAgent;

        }
    }
}
