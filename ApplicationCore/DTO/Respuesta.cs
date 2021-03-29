using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.DTO
{
    public class Respuesta<T>
    {
        public IDictionary<string, string> Headers { get; set; }
        public int HttpStatus { get; set; }
        public T Body { get; set; }
        public string Mensaje { get; set; }

    }
}
