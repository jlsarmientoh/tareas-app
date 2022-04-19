using ApplicationCore.DTO;
using System.Threading.Tasks;

namespace Javeriana.Core.Interfaces.Messaging
{
    public interface IConsumer
    {
        Task ProcesarMensaje(Mensaje mensaje);
    }
}
