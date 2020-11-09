using ApplicationCore.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Javeriana.Core.Interfaces.Messaging
{
    public interface IConsumer
    {
        Task ProcesarMensaje(Mensaje mensaje);
    }
}
