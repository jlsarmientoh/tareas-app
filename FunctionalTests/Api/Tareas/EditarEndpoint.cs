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
    public class EditarEndpoint : IClassFixture<ApiTestFixture>
    {
        private const string RequestUriEdit = "api/tareas/{0}";
        private const string RequestUriNew = "api/tareas";
        private readonly HttpClient _client;

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        private readonly IRestClient<Tarea> _restClient;
        public EditarEndpoint(ApiTestFixture factory)
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
            var response = await _client.PutAsync(string.Format(RequestUriEdit, tarea.Id), jsonContent);
            response.EnsureSuccessStatusCode();
            var actualTarea = await GetTareaAsync(tarea.Id);

            Assert.NotNull(actualTarea);
            Assert.InRange(actualTarea.Id, 1L, 99L);
            Assert.Equal(expected.Name, actualTarea.Name);
            Assert.Equal(expected.IsComplete, actualTarea.IsComplete);
        }

        [Fact]
        public async Task ShouldReturnBadRequest()
        {
            var tarea = CrearNuevaTareaAsync();

            var jsonContent = PruebasHelper.GetNewInvalidTareaJson();
            var response = await _client.PutAsync(string.Format(RequestUriEdit, tarea.Id), jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ShouldReturnNotFound()
        {
            var jsonContent = PruebasHelper.GetNewTareaJson();
            var response = await _client.PutAsync(string.Format(RequestUriEdit, 1000), jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ShouldEditTareaWithRestClient()
        {
            var tarea = CrearNuevaTareaAsync();
            Tarea expected = new Tarea
            {
                Id = tarea.Id,
                Name = PruebasHelper._name,
                IsComplete = true
            };

            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUriEdit)
            {
                Body = expected
            };
            peticion.PathVariables.Add(tarea.Id.ToString());

            Respuesta<Tarea> respuesta = await _restClient.PutAsync<Tarea>(peticion);
            var actualTarea = await GetTareaAsync(expected.Id);

            Assert.NotNull(respuesta);
            Assert.Equal(200, respuesta.HttpStatus);
            Assert.NotNull(actualTarea);
            Assert.Equal(expected.Id, actualTarea.Id);
            Assert.Equal(expected.Name, actualTarea.Name);
            Assert.Equal(expected.IsComplete, actualTarea.IsComplete);

        }

        [Fact]
        public async Task ShouldFailOnEditTareaWithRestClient()
        {
            var tarea = CrearNuevaTareaAsync();
            Peticion<Tarea> peticion = new Peticion<Tarea>(RequestUriEdit)
            {
                Body = JsonSerializer.Deserialize<Tarea>(await PruebasHelper.GetNewInvalidTareaJson().ReadAsStringAsync())
            };
            peticion.PathVariables.Add(tarea.Id.ToString());

            Respuesta<Tarea> respuesta = await _restClient.PutAsync<Tarea>(peticion);

            Assert.NotNull(respuesta);
            Assert.Equal(400, respuesta.HttpStatus);
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

        private async Task<Tarea> GetTareaAsync(long id)
        {
            var response = await _client.GetAsync(string.Format(RequestUriEdit, id));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Tarea>(stringResponse);
        }
    }
}
