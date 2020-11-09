using Javeriana.Api.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTO
{
    public class Mensaje
    {
        public DateTime FechaEnvio { get; set; }

        public Tarea Tarea { get; set; }
    }
}
