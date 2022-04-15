using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
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

namespace FunctionalTests.Api.Tareas
{
    [Collection("Sequential")]
    public class EliminarEndpoint : IClassFixture<ApiTestFixture>
    {
        private const string RequestUriDelete = "api/tareas/{0}";
        private const string RequestUriNew = "api/tareas";
        private readonly HttpClient _client;

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        private readonly IRestClient<Tarea> _restClient;
        public EliminarEndpoint(ApiTestFixture factory)
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
                IsComplete = true
            };

            var tarea = await CrearNuevaTareaAsync();
            tarea.IsComplete = true;

            var jsonContent = PruebasHelper.GetTareaJson(tarea);
            var response = await _client.DeleteAsync(string.Format(RequestUriDelete, tarea.Id));
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnNotFound()
        {
            var jsonContent = PruebasHelper.GetNewTareaJson();
            var response = await _client.DeleteAsync(string.Format(RequestUriDelete, 1000));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldDeleteTareaWithRestClient()
        {
            var tarea = await CrearNuevaTareaAsync();
            Tarea expected = new Tarea
            {
                Id = tarea.Id,
                Name = PruebasHelper._name,
                IsComplete = true
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUriDelete);
            peticion.PathVariables.Add(expected.Id.ToString());

            Respuesta<Tarea> respuesta = await _restClient.DeleteAsync<Tarea>(peticion);

            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
        }

        [Fact]
        public async Task ShouldFailOnDeleteTareaWithRestClient()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUriDelete);
            peticion.PathVariables.Add("1000");

            Respuesta<Tarea> respuesta = await _restClient.DeleteAsync<Tarea>(peticion);

            Assert.NotNull(respuesta);
            Assert.Equal(404, respuesta.HttpStatus);
            Assert.Null(respuesta.Body);
            Assert.NotNull(respuesta.Mensaje);

        }

        private async Task<Tarea> CrearNuevaTareaAsync()
        {
            var jsonContent = PruebasHelper.GetNewTareaJson();
            var response = await _client.PostAsync(RequestUriNew, jsonContent);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Tarea>(stringResponse);
        }
    }
}
