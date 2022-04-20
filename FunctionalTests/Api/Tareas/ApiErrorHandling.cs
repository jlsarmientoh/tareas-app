using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Infrastructure.WebServices;
using Javeriana.Api.DTO;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Api.FunctionalTests
{
    [Collection("Sequential")]
    public class ApiErrorHandling : IClassFixture<ApiTestFixtureErrorHandling>
    {
        private const string RequestUri = "api/tareas";

        private readonly HttpClient _client;

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        private readonly IRestClient<Tarea> _restClient;
        public ApiErrorHandling(ApiTestFixtureErrorHandling factory)
        {
            _client = factory.CreateClient();
            _mockLogger = new Mock<ILogger<JSONRestClient<Tarea>>>().Object;
            _restClient = new JSONRestClient<Tarea>(_client, _mockLogger);
        }

        [Fact]
        public async Task ShouldReturn5000OnGet()
        {
            //Arrange
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri);
            //Act
            var response = await _restClient.GetAsync<Tarea>(peticion);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(500,response.HttpStatus);
            Assert.NotEmpty(response.Mensaje);
        }

        [Fact]
        public async Task ShouldReturn5000OnGetSingle()
        {
            //Arrange
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1");
            peticion.Body = new Tarea{
                Id = 1,
                Name = "Tarea 1",
                IsComplete = false
            };
            //Act
            var response = await _restClient.GetAsync<Tarea>(peticion);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(500,response.HttpStatus);
            Assert.NotEmpty(response.Mensaje);
        }

        [Fact]
        public async Task ShouldReturn5000OnCreate()
        {
            //Arrange
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri);
            peticion.Body = new Tarea{
                Id = 1,
                Name = "Tarea 1",
                IsComplete = false
            };
            //Act
            var response = await _restClient.PostAsync<Tarea>(peticion);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(500,response.HttpStatus);
            Assert.NotEmpty(response.Mensaje);
        }

        [Fact]
        public async Task ShouldReturn5000OnUpdate()
        {
            //Arrange
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1");
            peticion.Body = new Tarea{
                Id = 1,
                Name = "Tarea 1",
                IsComplete = false
            };
            //Act
            var response = await _restClient.PutAsync<Tarea>(peticion);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(500,response.HttpStatus);
            Assert.NotEmpty(response.Mensaje);
        }

        [Fact]
        public async Task ShouldReturn5000OnDelete()
        {
            //Arrange
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri + "/1");
            //Act
            var response = await _restClient.DeleteAsync<Tarea>(peticion);
            //Assert
            Assert.NotNull(response);
            Assert.Equal(500,response.HttpStatus);
            Assert.NotEmpty(response.Mensaje);
        }
    }
}
