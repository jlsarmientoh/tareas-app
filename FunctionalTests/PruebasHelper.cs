using Javeriana.Api.DTO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FunctionalTests
{
    public class PruebasHelper
    {
        public static readonly string _name = "Tarea de prueba";
        public static readonly bool _isComplete = false;
        public static StringContent GetNewTareaJson()
        {
            var nuevaTarea = new Tarea()
            {
                Name = _name,
                IsComplete = _isComplete
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize<Tarea>(nuevaTarea), Encoding.UTF8, "application/json");

            return jsonContent;
        }

        public static StringContent GetNewInvalidTareaJson()
        {
            var nuevaTarea = new Tarea()
            {
                Name = "",
                IsComplete = _isComplete
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize<Tarea>(nuevaTarea), Encoding.UTF8, "application/json");

            return jsonContent;
        }

        public static StringContent GetTareaJson(Tarea tarea)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize<Tarea>(tarea), Encoding.UTF8, "application/json");

            return jsonContent;
        }
    }
}
