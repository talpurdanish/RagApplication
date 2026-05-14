namespace RagWebApi.Viewmodels
{
    public class ChatCompletionResponse
    {
        public string Id { get; set; } = string.Empty;
        public string @Object { get; set; } = string.Empty;
        public long Created { get; set; }
        public string Model { get; set; } = string.Empty;
        public Choice[] Choices { get; set; } = [];
        public Usage? Usage { get; set; }
        public string System_fingerprint { get; set; } = string.Empty;
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message? Message { get; set; }
        public string Finish_reason { get; set; } = string.Empty;
    }

    public class Message
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Image[] Images { get; set; } = [];
    }

    public class Usage
    {
        public int Prompt_tokens { get; set; }
        public int Completion_tokens { get; set; }
        public int Total_tokens { get; set; }
    }
}
