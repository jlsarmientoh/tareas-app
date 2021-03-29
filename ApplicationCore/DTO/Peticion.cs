using System.Collections;
using System.Collections.Generic;

namespace ApplicationCore.DTO
{
    public class Peticion<T>
    {
        public readonly IDictionary<string, string> Headers;

        public readonly IDictionary<string, string> Params;

        public readonly IList<string> PathVariables;
        public T Body { get; set; }

        public string Endpoint;

        public Peticion(string endpoint)
        {
            Endpoint = endpoint;
            Headers = new Dictionary<string, string>();
            Params = new Dictionary<string, string>();
            PathVariables = new List<string>();
        }
    }
}