using System.Collections;
using System.Collections.Generic;
using System.Text;

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

        public string ResolverRequestURL()
        {
            string[] vars = ((List<string>)this.PathVariables).ToArray();
            string formatedEndpoint = string.Format(this.Endpoint, vars);
            StringBuilder stringBuilder = new StringBuilder(formatedEndpoint);
            
            if (this.Params.Count > 0)
            {
                int index = 0;
                foreach (var param in this.Params)
                {
                    stringBuilder.Append((index == 0) ? "?" : "&");
                    stringBuilder.Append($"{param.Key}={param.Value}");
                    index++;
                }
            }

            return stringBuilder.ToString();
        }
    }
}