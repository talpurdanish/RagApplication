using Moq;
using Moq.Protected;
using RagWebApi.Service;
using System.Net;
using System.Text;


namespace RagWebApi.UnitTests.Service
{
    [TestFixture]
    internal class ChatServiceTests
    {

        private Mock<HttpMessageHandler>? _handlerMock;
        private HttpClient? _httpClient;
        private ChatService? _chatService;

        [SetUp]
        public void Setup()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object);
            _chatService = new ChatService(_httpClient, _httpClient);
        }

        [Test]
        public async Task GetResponse_ReturnsSuccess_WhenReplyExists()
        {
            var json = @"{
            ""choices"": [
                { ""message"": { ""content"": ""Hello world"" } }
            ]
        }";

            _handlerMock!
                 .Protected() // access protected members
                 .Setup<Task<HttpResponseMessage>>(
                     "SendAsync", // method name
                     ItExpr.IsAny<HttpRequestMessage>(),
                     ItExpr.IsAny<CancellationToken>()
                 )
                 .ReturnsAsync(new HttpResponseMessage
                 {
                     StatusCode = HttpStatusCode.OK,
                     Content = new StringContent(json, Encoding.UTF8, "application/json")
                 });


            var (success, reply) = await _chatService!.GetResponse(new List<ChatQuery>
            {
                new (ChatMessageType.text, "Hi")
            });

            Assert.IsTrue(success);
            Assert.AreEqual("Hello world", reply);
        }

        [Test]
        public async Task GetResponse_ReturnsFalse_WhenReplyIsEmpty()
        {
            var json = @"{ ""choices"": [] }";

            _handlerMock!
            .Protected() // access protected members
            .Setup<Task<HttpResponseMessage>>(
                  "SendAsync", // method name
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
              )
              .ReturnsAsync(new HttpResponseMessage
              {
                  StatusCode = HttpStatusCode.OK,
                  Content = new StringContent(json, Encoding.UTF8, "application/json")
              });

            var (success, reply) = await _chatService!.GetResponse(new List<ChatQuery>
        {
            new (ChatMessageType.text, "Hi")
        });

            Assert.IsFalse(success);
            Assert.AreEqual("", reply);
        }

        [Test]
        public void GetResponse_ThrowsException_WhenHttpFails()
        {
            _handlerMock!
             .Protected() // access protected members
             .Setup<Task<HttpResponseMessage>>(
                 "SendAsync", // method name
                 ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>()
             )
            .ThrowsAsync(new HttpRequestException("Network error"));

            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _chatService!.GetResponse(new List<ChatQuery>
                {
                new (ChatMessageType.text, "Hi")
                }));
        }

        [Test]
        public async Task GetResponse_RetriesAndFails_WhenAlwaysBadStatus()
        {
            _handlerMock!
            .Protected() // access protected members
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", // method name
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            });

            
            Assert.ThrowsAsync<HttpRequestException>(async () => await _chatService!.GetResponse(new List<ChatQuery>
                {
                    new (ChatMessageType.text, "Hi")
                }));

        }


        [Test]
        public async Task GenerateImage_ReturnsSuccess_WhenApiRespondsOk()
        {
            var fakeBytes = Encoding.UTF8.GetBytes("fake-image-data");

            _handlerMock!.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ByteArrayContent(fakeBytes)
                });

            var (success, data) = await _chatService!.GenerateImage("banana");

            Assert.IsTrue(success);
            Assert.AreEqual(fakeBytes, data);
        }

        [Test]
        public async Task GenerateImage_ReturnsFalse_WhenApiRespondsError()
        {
            _handlerMock!.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var (success, data) = await _chatService!.GenerateImage("banana");

            Assert.IsFalse(success);
            Assert.AreEqual(0, data.Length);
        }

        [Test]
        public void GenerateImage_ThrowsException_WhenHttpFails()
        {
            _handlerMock!.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Network error"));

            Assert.ThrowsAsync<HttpRequestException>(async () =>
                await _chatService!.GenerateImage("banana"));
        }

        [Test]
        public async Task GenerateImage_RetriesAndFails_WhenAlwaysBadStatus()
        {
            _handlerMock!.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var (success, data) = await _chatService!.GenerateImage("banana");

            Assert.IsFalse(success);
            Assert.AreEqual(0, data.Length);

            // Verify retry count (3 attempts)
            _handlerMock!.Protected()
                .Verify("SendAsync",
                    Times.Exactly(3),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>());
        }

    }
}