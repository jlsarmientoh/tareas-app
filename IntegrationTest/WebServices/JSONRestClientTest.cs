using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using Javeriana.Api.DTO;
using ApplicationCore.DTO;
using Microsoft.Extensions.Logging;
using Infrastructure.WebServices;
using Moq;
using Moq.Protected;
using System.Threading;

namespace IntegrationTest
{
    public class JSONRestClientTest
    {

        private const string RequestUri = "http://test/api/tareas";

        private const string _contentType = "application/json";

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        public JSONRestClientTest()
        {
            _mockLogger = new Mock<ILogger<JSONRestClient<Tarea>>>().Object;
        }

        [Fact]
        public async Task ShouldCreateNewTareaWithRestClient()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = "Test valid RestClient",
                IsComplete = false
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri)
            {
                Body = expected,
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);
            Respuesta<Tarea> respuesta = await restClient.PostAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());
            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
            Assert.NotNull(respuesta.Body);
            Assert.InRange(respuesta.Body.Id, 1L, 99L);
            Assert.Equal(expected.Name, respuesta.Body.Name);
            Assert.Equal(expected.IsComplete, respuesta.Body.IsComplete);
        }

        [Fact]
        public async Task ShouldFailOnCreateNewTareaWithRestClient()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri)
            {
                Body = new Tarea
                {
                    Id = 1,
                    Name = "Test invalid RestClient",
                    IsComplete = false
                }
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);

            Respuesta<Tarea> respuesta = await restClient.PostAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());            
            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }

        [Fact]
        public async Task ShouldUpdateTareaWithRestClient()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = "Test valid update RestClient",
                IsComplete = true
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/" + expected.Id)
            {
                Body = expected,
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);
            Respuesta<Tarea> respuesta = await restClient.PutAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>());
            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
            Assert.NotNull(respuesta.Body);
            Assert.InRange(respuesta.Body.Id, 1L, 99L);
            Assert.Equal(expected.Name, respuesta.Body.Name);
            Assert.Equal(expected.IsComplete, respuesta.Body.IsComplete);
        }

        [Fact]
        public async Task ShouldFailOnUpdatingTareaWithRestClient()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1")
            {
                Body = new Tarea()
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);

            Respuesta<Tarea> respuesta = await restClient.PutAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>());            
            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }

        [Fact]
        public async Task ShouldDeleteTareaWithRestClient()
        {
            Tarea expected = new Tarea();

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/" + expected.Id)
            {
                Body = expected,
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);
            Respuesta<Tarea> respuesta = await restClient.DeleteAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>());
            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
            Assert.NotNull(respuesta.Body);
        }

        [Fact]
        public async Task ShouldFailOnDeletingTareaWithRestClient()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1")
            {
                Body = new Tarea()
            };

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);

            Respuesta<Tarea> respuesta = await restClient.DeleteAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete),
                ItExpr.IsAny<CancellationToken>());            
            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }

        [Fact]
        public async Task ShouldGetSinlgeTareaWithRestClient()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = "Test valid RestClient",
                IsComplete = false
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1");

            var json = JsonSerializer.Serialize<Tarea>(expected);

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);
            Respuesta<Tarea> respuesta = await restClient.GetAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());
            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
            Assert.NotNull(respuesta.Body);
            Assert.InRange(respuesta.Body.Id, 1L, 99L);
            Assert.Equal(expected.Name, respuesta.Body.Name);
            Assert.Equal(expected.IsComplete, respuesta.Body.IsComplete);
        }

        [Fact]
        public async Task ShouldFailOnGettingSingleTareaWithRestClient()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1");

            var json = JsonSerializer.Serialize<Tarea>(peticion.Body);
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(json),
                    Headers = { { "X-Test-Header", "Test-Value" } }
                });

            var restClient = new JSONRestClient<Tarea>(new HttpClient(mockHandler.Object), _mockLogger);

            Respuesta<Tarea> respuesta = await restClient.GetAsync<Tarea>(peticion);
                        
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());            
            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }
    }
}
