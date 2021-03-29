using Javeriana.Api.DTO;
using System;

namespace ApplicationCore.DTO
{
    public class Mensaje
    {
        public DateTime FechaEnvio { get; set; }

        public Tarea Tarea { get; set; }
    }
}
