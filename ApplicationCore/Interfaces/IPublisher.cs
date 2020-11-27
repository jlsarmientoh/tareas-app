using ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Javeriana.Core.Interfaces.Messaging
{
    public interface IPublisher
    {
        void PublicarMensaje(Mensaje mensaje);
        void DistribuirMensaje(Mensaje mensaje);
        Task PublicarMensajeAsync(Mensaje mensaje);
        Task DistribuirMensajeAsync(Mensaje mensaje);
    }
}
