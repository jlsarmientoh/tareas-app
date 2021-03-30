using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.WebServices
{
    public class JSONRestClient<T> : IRestClient<T>
    {

        private readonly HttpClient _client;

        private readonly string _contentType = "application/json";

        private readonly ILogger<JSONRestClient<T>> _logger;

        public JSONRestClient(HttpClient client, ILogger<JSONRestClient<T>> logger)
        {
            _client = client;
            _logger = logger;
        }

        public Task<Respuesta<T>> Delete(Peticion<T> peticion)
        {
            throw new NotImplementedException();
        }

        public async Task<Respuesta<T>> Get(Peticion<T> peticion)
        {
            Respuesta<T> respuesta = new Respuesta<T>();
            setHeaders(peticion);
            try
            {
                var response = await _client.GetAsync(peticion.ResolverRequestURL());
                respuesta.HttpStatus = (int)response.StatusCode;
                response.EnsureSuccessStatusCode();
                var msg = await response.Content.ReadAsStringAsync();

                foreach (var header in response.Headers)
                {
                    _logger.LogDebug($"{header.Key} : {string.Join(" ", header.Value)}");
                    respuesta.Headers.Add(header.Key, string.Join(" ", header.Value));
                }

                respuesta.Body = await JsonSerializer.DeserializeAsync<T>(await response.Content?.ReadAsStreamAsync());

            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Message :{ex.Message}");
                respuesta.Mensaje = ex.Message;
            }
            catch (FormatException ex)
            {
                _logger.LogError($"Message :{ex.Message}");
                respuesta.Mensaje = "Petición inválida";
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Message :{ex.Message}");
                respuesta.Mensaje = "No se pudo obtener el cuerpo de la respuesta";
            }

            return respuesta;
        }

        public Task<Respuesta<T>> Post(Peticion<T> peticion)
        {
            throw new NotImplementedException();
        }

        public Task<Respuesta<T>> Put(Peticion<T> peticion)
        {
            throw new NotImplementedException();
        }

        private void setHeaders(Peticion<T> peticion)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(_contentType));
            _client.DefaultRequestHeaders.Add("User-Agent", "Tareas App");

            foreach(var header in peticion.Headers)
            {
                _client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }
}
