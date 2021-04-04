using ApplicationCore.DTO;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IRestClient<T>
    {
        Task<Respuesta<TBody>> GetAsync<TBody>(Peticion<T> peticion);

        Task<Respuesta<TBody>> PostAsync<TBody>(Peticion<T> peticion);

        Task<Respuesta<TBody>> PutAsync<TBody>(Peticion<T> peticion);

        Task<Respuesta<TBody>> DeleteAsync<TBody>(Peticion<T> peticion);
    }
}
