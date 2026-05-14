using Microsoft.Extensions.AI; // assuming Embedding<T> comes from here
using Moq;
using Jina;
using RagWebApi.Service.Movies;
using RagWebApi.Service;


namespace RagWebApi.UnitTests.Service.Movies
{
    

    [TestFixture]
    public class MovieAiServiceTests
    {
        private Mock<IJinaClientWrapper>? _jinaClientMock;
        private MovieAiService? _service;

        [SetUp]
        public void Setup()
        {
            _jinaClientMock = new Mock<IJinaClientWrapper>();
            _service = new MovieAiService(_jinaClientMock.Object);
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_Text_ReturnsSuccess()
        {
            var fakeEmbedding = new Embedding<float>(new float[] { 0.1f, 0.2f });

            _jinaClientMock!.Setup(c => c.GenerateAsync(
                It.IsAny<string>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ReturnsAsync(fakeEmbedding);

            var (success, embedding) = await _service!.GenerateJinaEmbeddingsAsync("hello");

            Assert.IsTrue(success);
            Assert.NotNull(embedding);
            Assert.AreEqual(fakeEmbedding, embedding);
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_Text_ReturnsFalse_OnException()
        {
            _jinaClientMock!.Setup(c => c.GenerateAsync(
                It.IsAny<string>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ThrowsAsync(new System.Exception("API error"));

            var (success, embedding) = await _service!.GenerateJinaEmbeddingsAsync("hello");

            Assert.IsFalse(success);
            Assert.IsNull(embedding);

            // Verify retries (3 attempts)
            _jinaClientMock!.Verify(c => c.GenerateAsync(It.IsAny<string>(), It.IsAny<EmbeddingGenerationOptions>()),
                Times.Exactly(3));
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_TextWithMovieId_ReturnsSuccess()
        {
            var fakeEmbedding = new Embedding<float>(new float[] { 0.3f, 0.4f });

            _jinaClientMock!.Setup(c => c.GenerateAsync(
                It.IsAny<string>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ReturnsAsync(fakeEmbedding);

            var (success, embeddings) = await _service!.GenerateJinaEmbeddingsAsync("movie text", 123);

            Assert.IsTrue(success);
            Assert.NotNull(embeddings);
            Assert.AreEqual(1, embeddings!.Count);
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_TextArray_ReturnsSuccess()
        {
            var fakeEmbeddings = new GeneratedEmbeddings<Embedding<float>>(
                new[] { new Embedding<float>(new float[] { 0.5f, 0.6f }) });

            _jinaClientMock!.Setup(c => c.GenerateMixedEmbeddingsAsync(
                It.IsAny<AnyOf<string, TextDoc, ImageDoc>[]>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ReturnsAsync(fakeEmbeddings);

            var (success, result) = await _service!.GenerateJinaEmbeddingsAsync(new[] { "text1", "text2" }, 456);

            Assert.IsTrue(success);
            Assert.NotNull(result);
            Assert.AreEqual(1, result!.Count);
        }

        [Test]
        public async Task GenerateJinaEmbeddingsAsync_TextArray_ReturnsFalse_OnException()
        {
            _jinaClientMock!.Setup(c => c.GenerateMixedEmbeddingsAsync(
                It.IsAny<AnyOf<string, TextDoc, ImageDoc>[]>(),
                It.IsAny<EmbeddingGenerationOptions>()))
                .ThrowsAsync(new System.Exception("API error"));

            var (success, result) = await _service!.GenerateJinaEmbeddingsAsync(new[] { "text1" }, 789);

            Assert.IsFalse(success);
            Assert.IsNull(result);

            _jinaClientMock!.Verify(c => c.GenerateMixedEmbeddingsAsync(
                It.IsAny<AnyOf<string, TextDoc, ImageDoc>[]>(),
                It.IsAny<EmbeddingGenerationOptions>()),
                Times.Exactly(3));
        }
    }
}
