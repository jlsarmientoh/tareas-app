using ApplicationCore.DTO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRestClient<T,R>
    {
        Task<Respuesta<R>> Get(Peticion<T> peticion);

        Task<Respuesta<R>> Post(Peticion<T> peticion);

        Task<Respuesta<R>> Put(Peticion<T> peticion);

        Task<Respuesta<R>> Delete(Peticion<T> peticion);
    }
}
