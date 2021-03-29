using ApplicationCore.DTO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRestClient<T>
    {
        Task<Respuesta<T>> Get(Peticion<T> peticion);

        Task<Respuesta<T>> Post(Peticion<T> peticion);

        Task<Respuesta<T>> Put(Peticion<T> peticion);

        Task<Respuesta<T>> Delete(Peticion<T> peticion);
    }
}
