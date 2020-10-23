using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javeriana.Core.Interfaces
{
    public interface IUnitOfWork
    {
        void Confirmar();

        Task ConfirmarAsync();
    }
}
