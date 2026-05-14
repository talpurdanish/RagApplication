using Newtonsoft.Json;
using RagWebApi.Viewmodels;
using System.Text;

namespace RagWebApi.Service
{

    public interface IChatService
    {

        Task<(bool, string)> GetResponse(List<ChatQuery> queries);
        Task<(bool, byte[])> GenerateImage(string prompt, string model);

    }

    public enum ChatMessageType
    {

        text, imageurl

    }

    public record ChatQuery(ChatMessageType Type, string Message);


    public class ChatService(HttpClient client, HttpClient imageClient) : IChatService
    {
        private const string endpoint = "https://openrouter.ai/api/v1/chat/completions";
        private const string imageEndpoint = "https://api.cloudflare.com/client/v4/accounts/6a688576791cd5f8a451879339f16545/ai/run/";

        public async Task<(bool, string)> GetResponse(List<ChatQuery> queries)
        {
            int retry = 0;
            do
            {
                try
                {

                    var content = new object[queries.Count];
                    var index = 0;
                    foreach (var query in queries)
                    {
                        if (query.Type == ChatMessageType.text)
                        {
                            content[index] = new { type = "text", text = query.Message };
                        }
                        else
                        {

                            content[index] = new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = query.Message
                                }
                            };
                        }
                        index++;
                    }

                    var payload = new
                    {
                        messages = new[]
                        {
                                new {
                                    role = "user",
                                    content,
                                }
                            },
                        reasoning = new
                        {
                            enabled = true
                        },

                        model = "openrouter/free",
                        stream = false
                    };
                    string json = JsonConvert.SerializeObject(payload);

                    var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),

                    };


                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(jsonString))
                        {
                            var jsonObject = JsonConvert.DeserializeObject<ChatCompletionResponse>(jsonString);
                            var reply = jsonObject?.Choices?.Length > 0 ? jsonObject?.Choices?[0]?.Message?.Content : "";
                            if (!string.IsNullOrEmpty(reply))
                            {
                                return (true, reply);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;

                    throw;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, "");
        }

        public async Task<(bool, byte[])> GenerateImage(string prompt, string model = "@cf/stabilityai/stable-diffusion-xl-base-1.0")
        {
            var endPoint = $"{imageEndpoint}{model}";
            int retry = 0;
            do
            {
                try
                {
                    var payload = new
                    {
                        prompt
                    };
                    string json = JsonConvert.SerializeObject(payload);

                    Console.WriteLine(json);

                    var request = new HttpRequestMessage(HttpMethod.Post, endPoint)
                    {
                        Content = new StringContent(json, Encoding.UTF8, "application/json")

                    };

                    var response = await imageClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    //response.EnsureSuccessStatusCode();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsByteArrayAsync();

                        return (true, jsonString);

                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;

                    throw;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, []);
        }
    }

}
