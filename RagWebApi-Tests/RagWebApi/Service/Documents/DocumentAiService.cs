using Jina;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using RagWebApi.Models;
using System.Text;

namespace RagWebApi.Service.Documents
{

    public interface IDocumentAiService
    {
        Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text);
        Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(List<string> documentData, List<int> documentId);

        Task<string> SearchAsync(string query, List<DataDocument> documents, List<string> previousUserPrompts);


    }
    public class DocumentAiService(JinaClient jinaClient, IChatService chatService) : IDocumentAiService
    {


        public async Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text)
        {

            var (success, embeddings) = await GenerateJinaEmbeddingsAsync([text], []);
            if (embeddings != null)
                return (success, embeddings.FirstOrDefault());
            else
                return (false, null);
        }

        public async Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(List<string> documentData, List<int> documentId)
        {
            int retry = 0;

            do
            {
                if (documentId.Count > 0)
                {
                    string documentIds = string.Join(", ", documentId);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Processing Document with Ids [{documentIds}] retry attempt {retry + 1}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                try
                {

                    IEnumerable<AnyOf<string, TextDoc, ImageDoc>> items = documentData.Select(d => new AnyOf<string, TextDoc, ImageDoc>(new TextDoc { Text = d })).ToList();

                    var embeddings = await jinaClient.GenerateMixedEmbeddingsAsync(
                                            items,
                                            new EmbeddingGenerationOptions
                                            {
                                                Dimensions = 512,
                                            });

                    return (true, embeddings);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, null);
        }
        private static double CosineSimilarity(float[] v1, float[]? v2)
        {
            if (v1 is null || v2 is null)
                return 0;
            double dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }
            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }

        public async Task<string> SearchAsync(string query, List<DataDocument> documents, List<string> previousUserPrompts)
        {
            try
            {
                var queryEmbedding = await jinaClient.GenerateAsync(query);
                var results = documents
                    .Select(p => new
                    {
                        Document = p,
                        Similarity = p.DescriptionEmbedding != null ? CosineSimilarity(queryEmbedding.Vector.ToArray(), JsonConvert.DeserializeObject<float[]>(p.DescriptionEmbedding)) : 10000
                    })

                    .OrderByDescending(r => r.Similarity)
                    .Take(3)
                    .ToList();

                var chatHistory = new ChatHistory()
                {
                    PreviousUserPrompts = previousUserPrompts,
                    DocumentVectors = results.Select(r => r.Document).OrderBy(r => r.Id).ToList()
                };

                var prompt = chatHistory.BuildPrompt(query);

                var chatMessages = new List<ChatQuery>(){
                    new(ChatMessageType.text, prompt)
                };

                var (success, response) = await chatService.GetResponse(chatMessages);

                if (success)
                {
                    return response;
                }

                return "";

            }
            catch (Exception)
            {

                throw;
            }
        }


    }

    public class ChatHistory
    {
        public List<string> PreviousUserPrompts { get; set; } = [];
        public List<DataDocument> DocumentVectors { get; set; } = [];
        public string BuildPrompt(string currentUserPrompt)
        {
            var sb = new StringBuilder();

            sb.AppendLine("You are a helpful AI assistant. Use the context, previous user queries and the current user query to generate a coherent, context-aware response.");
            sb.AppendLine();
            sb.AppendLine("Context:");

            for (int i = 0; i < DocumentVectors.Count; i++)
            {
                sb.AppendLine($"Document: {DocumentVectors[i].ToString()}");
            }
            sb.AppendLine("Previous user Queries:");

            for (int i = 0; i < PreviousUserPrompts.Count; i++)
            {
                sb.AppendLine($"User: {PreviousUserPrompts[i].ToString()}");
            }

            sb.AppendLine();
            sb.AppendLine("Current Question:");
            sb.AppendLine(currentUserPrompt);
            sb.AppendLine();
            sb.AppendLine("Instructions:");
            sb.AppendLine("- Respond naturally, continuing the conversation.");
            sb.AppendLine("- Use the history to maintain context and avoid repetition.");
            sb.AppendLine("- If the answer is not clear from history, rely on general knowledge.");
            sb.AppendLine("- Keep the response concise and relevant.");

            return sb.ToString();
        }

    }

}
