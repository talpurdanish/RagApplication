using Jina;
using Microsoft.Extensions.AI;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;


namespace RagWebApi.Service.Images
{

    public interface IImageAiService
    {
        Task<(bool, string)> GetAiInsights(int imageId);
        Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text);
        Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(byte[]? imageData, string? aiInsight, int imageId = -1);

        Task<byte[]> GenerateImage(string query, string model );
    }
    public class ImageAiService(IJinaClientWrapper jinaClient, IChatService chatService) : IImageAiService
    {

        public async Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text)
        {

            var (success, embeddings) = await GenerateJinaEmbeddingsAsync(null, text);
            if (embeddings != null)
                return (success, embeddings.FirstOrDefault());
            else
                return (false, null);
        }

        public async Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(byte[]? imageData, string? aiInsight, int imageId = -1)
        {
            int retry = 0;
            if (imageData == null && aiInsight == null)
            {
                throw new ArgumentException("Both imageData and aiInsight cannot be null.");
            }
            do
            {
                if (imageId > -1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Processing Image with Id {imageId} retry attempt {retry + 1}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                try
                {

                    List<AnyOf<string, TextDoc, ImageDoc>> items = [];
                    if (imageData != null)
                    {
                        var base64 = GenerateSmallImage(imageData);
                        items.Add(new AnyOf<string, TextDoc, ImageDoc>(new ImageDoc { Image = base64 }));
                    }

                    if (aiInsight != null)
                    {
                        items.Add(new AnyOf<string, TextDoc, ImageDoc>(new TextDoc { Text = aiInsight }));
                    }

                    var embeddings = await jinaClient.GenerateMixedEmbeddingsAsync(
                                            items.ToArray(),
                                            new EmbeddingGenerationOptions
                                            {
                                                Dimensions = 256,
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


        public async Task<(bool, string)> GetAiInsights(int imageId)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"Processing Image with Id {imageId}");
            Console.ForegroundColor = ConsoleColor.White;
            try
            {

                var chatMessages = new List<ChatQuery>() {
                        new(ChatMessageType.text, "Describe this image in two sentence."),
                        new(ChatMessageType.imageurl, $"https://golden-ram-flying.ngrok-free.app/api/images/GetPicture/{imageId}"),
                    };

                return await chatService.GetResponse(chatMessages);

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {e.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }


            return (false, "");
        }

        public async Task<byte[]> GenerateImage(string query, string model)
        {
            var (success, list) = await chatService.GenerateImage(query, model );
            return list;
        }

        public string GenerateSmallImage(byte[] imageData)
        {
            using MemoryStream inputStream = new(imageData);
            using Image image = Image.Load(inputStream);
            image.Mutate(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max, // crop instead of just shrinking
                    Size = new Size(512, 512),
                });
            });

            using var ms = new MemoryStream();
            image.SaveAsJpeg(ms, new JpegEncoder { Quality = 75 });

            return Convert.ToBase64String(ms.ToArray());
        }

    }



}
