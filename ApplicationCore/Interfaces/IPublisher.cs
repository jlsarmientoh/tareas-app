using ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Javeriana.Core.Interfaces.Messaging
{
    public interface IPublisher
    {
        void PublicarMensaje(Mensaje mensaje);

        void DistribuirMensaje(string mensaje);
    }
}
