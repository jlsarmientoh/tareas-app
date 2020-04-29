using System;
using System.Runtime.Serialization;

namespace Javeriana.Api.Exceptions
{
    public class TareaNoExisteException : Exception
    {
        public TareaNoExisteException()
        {
        }

        public TareaNoExisteException(string message) : base(message)
        {
        }

        public TareaNoExisteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TareaNoExisteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}