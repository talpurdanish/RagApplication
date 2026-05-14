
using Jina;
using Microsoft.Extensions.AI;
using Moq;
using RagWebApi.Service;
using RagWebApi.Service.Images;
using System.Text;
namespace RagWebApi.UnitTests.Service.Images
{

    [TestFixture]
    public class ImageAiServiceTests
    {
        private Mock<IJinaClientWrapper>? _jinaMock;
        private Mock<IChatService>? _chatMock;
        private ImageAiService? _service;

        [SetUp]
        public void Setup()
        {
            _jinaMock = new Mock<IJinaClientWrapper>();
            _chatMock = new Mock<IChatService>();
            _service = new ImageAiService(_jinaMock.Object, _chatMock.Object);
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_Text_ReturnsSuccess()
        {
            var fakeEmbedding = new Embedding<float>(new float[] { 0.1f, 0.2f });
            var fakeEmbeddings = new GeneratedEmbeddings<Embedding<float>>(new[] { fakeEmbedding });

            _jinaMock!.Setup(j => j.GenerateMixedEmbeddingsAsync(
                It.IsAny<AnyOf<string, TextDoc, ImageDoc>[]>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ReturnsAsync(fakeEmbeddings);

            var (success, embedding) = await _service!.GenerateJinaEmbeddingsAsync("hello");

            Assert.IsTrue(success);
            Assert.NotNull(embedding);
            Assert.AreEqual(fakeEmbedding, embedding);
        }

        [Test]
        public void GenerateJinaEmbeddingsAsync_BothNull_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await _service!.GenerateJinaEmbeddingsAsync(null, null));
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_WithAiInsight_ReturnsSuccess()
        {
            var fakeEmbeddings = new GeneratedEmbeddings<Embedding<float>>(
                new[] { new Embedding<float>(new float[] { 0.3f, 0.4f }) });

            _jinaMock!.Setup(j => j.GenerateMixedEmbeddingsAsync(
                It.IsAny<AnyOf<string, TextDoc, ImageDoc>[]>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ReturnsAsync(fakeEmbeddings);

            var (success, result) = await _service!.GenerateJinaEmbeddingsAsync(null, "insight", 123);

            Assert.IsTrue(success);
            Assert.NotNull(result);
            Assert.AreEqual(1, result!.Count);
        }

        [Test]
        public async Task GetAiInsights_ReturnsSuccess()
        {
            _chatMock!.Setup(c => c.GetResponse(It.IsAny<List<ChatQuery>>()))
                .ReturnsAsync((true, "This is an image"));

            var (success, insight) = await _service!.GetAiInsights(42);

            Assert.IsTrue(success);
            Assert.AreEqual("This is an image", insight);
        }

        [Test]
        public async Task GetAiInsights_ReturnsFalse_OnException()
        {
            _chatMock!.Setup(c => c.GetResponse(It.IsAny<List<ChatQuery>>()))
                .ThrowsAsync(new Exception("API error"));

            var (success, insight) = await _service!.GetAiInsights(42);

            Assert.IsFalse(success);
            Assert.AreEqual("", insight);
        }

        [Test]
        public async Task GenerateImage_ReturnsBytes()
        {
            var fakeBytes = Encoding.UTF8.GetBytes("image-data");
            _chatMock!.Setup(c => c.GenerateImage("query", "model"))
                .ReturnsAsync((true, fakeBytes));

            var result = await _service!.GenerateImage("query", "model");

            Assert.AreEqual(fakeBytes, result);
        }

        [Test]
        public void GenerateSmallImage_ReturnsBase64()
        {
            // Create a small dummy byte array representing an image
            var imageBytes = File.ReadAllBytes("test.jpg"); // or generate dynamically in-memory
            var base64 = _service!.GenerateSmallImage(imageBytes);

            Assert.IsNotNull(base64);
            Assert.IsTrue(base64.Length > 0);
        }
    }
}
