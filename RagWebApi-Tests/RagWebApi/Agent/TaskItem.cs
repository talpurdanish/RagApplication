using System.Text.Json.Serialization;

namespace RagWebApi.Agent
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool IsDone { get; set; }
    }

    public record RagAgentResponse {


        [property: JsonPropertyName("success")] public bool Success { get; set; } = false;
        [property: JsonPropertyName("message")] public string Message { get; set; } = string.Empty;
        [property: JsonPropertyName("data")] public TaskItem[] Data { get; set; } = [];
        [property: JsonPropertyName("completed")] public bool Completed { get; set; } = false;

        public static RagAgentResponse ErrorResponse(string message) {

            return new RagAgentResponse()
            {
                Success = false,
                Message = message
            };

        }
    }

    public class AgentWorkflowState { 
    
    
        public string UserInput { get; set; } = string.Empty;
        public string RouteChoice { get; set; } = string.Empty;

        public RagAgentResponse? FinalResponse { get; set; }

    }
}
