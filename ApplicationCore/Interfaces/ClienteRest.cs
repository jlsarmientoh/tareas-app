using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface ClienteRest<T>
    {
        T Get();

        void Post(T body);

        void Put(T body);

        void Delete(T body);
    }
}
