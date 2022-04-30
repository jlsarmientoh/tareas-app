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
    public class GetEndpoint : IClassFixture<ApiTestFixture>
    {
        private const string RequestUriEdit = "api/tareas/{0}";
        private const string RequestUriNew = "api/tareas";
        private readonly HttpClient _client;

        private readonly ILogger<JSONRestClient<Tarea>> _mockLogger;

        private readonly IRestClient<Tarea> _restClient;
        public GetEndpoint(ApiTestFixture factory)
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
                IsComplete = false
            };

            var tarea = await CrearNuevaTareaAsync();
            var actualTarea = await GetTareaAsync(tarea.Id);

            Assert.NotNull(actualTarea);
            Assert.InRange(actualTarea.Id, 1L, 99L);
            Assert.Equal(expected.Name, actualTarea.Name);
            Assert.Equal(expected.IsComplete, actualTarea.IsComplete);
        }

        [Fact]
        public async Task ShouldReturnListSuccess()
        {
            Tarea expected = new Tarea
            {
                Id = 1,
                Name = PruebasHelper._name,
                IsComplete = true
            };

            var tarea1 = await CrearNuevaTareaAsync();
            var tarea2 = await CrearNuevaTareaAsync();
            var tarea3 = await CrearNuevaTareaAsync();
            IEnumerable<Tarea> tareas = await GetTareasAsync();

            Assert.NotNull(tareas);
            Assert.NotEmpty(tareas);
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
            var response = await _client.GetAsync(string.Format(RequestUriEdit, 1000));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
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

        private async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            var response = await _client.GetAsync(RequestUriNew);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Tarea>>(stringResponse);
        }
    }
}
