using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using FunctionalTests.Api;
using Infrastructure.WebServices;
using Javeriana.Api.DTO;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.Api.FunctionalTests
{
    [Collection("Sequential")]
    public class CrearEndpoint : IClassFixture<ApiTestFixture>
    {
        private const string RequestUri = "api/tareas";
        private readonly HttpClient _client;

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        private readonly IRestClient<Tarea> _restClient;
        public CrearEndpoint(ApiTestFixture factory)
        {
            _client = factory.CreateClient();
            _mockLogger = new Mock<ILogger<JSONRestClient<Tarea>>>().Object;
            _restClient = new JSONRestClient<Tarea>(_client, _mockLogger);
        }

        [Fact]
        public async Task ShouldReturnSuccess()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = PruebasHelper._name,
                IsComplete = PruebasHelper._isComplete
            };
            var jsonContent = PruebasHelper.GetNewTareaJson();
            var response = await _client.PostAsync(RequestUri, jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var tarea = JsonSerializer.Deserialize<Tarea>(stringResponse);

            Assert.NotNull(tarea);
            Assert.InRange(tarea.Id, 1L, 99L);
            Assert.Equal(expected.Name, tarea.Name);
            Assert.Equal(expected.IsComplete, tarea.IsComplete);
        }

        [Fact]
        public async Task ShouldReturnBadRequest()
        {
            var jsonContent = PruebasHelper.GetNewInvalidTareaJson();
            var response = await _client.PostAsync(RequestUri, jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldCreateNewTareaWithRestClient()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = PruebasHelper._name,
                IsComplete = PruebasHelper._isComplete
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUri)
            {
                Body = JsonSerializer.Deserialize<Tarea>(await PruebasHelper.GetNewTareaJson().ReadAsStringAsync())
            };

            Respuesta<Tarea> respuesta = await _restClient.PostAsync<Tarea>(peticion);
                        
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
                Body = JsonSerializer.Deserialize<Tarea>(await PruebasHelper.GetNewInvalidTareaJson().ReadAsStringAsync())
            };

            Respuesta<Tarea> respuesta = await _restClient.PostAsync<Tarea>(peticion);
            
            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }

    }
}
