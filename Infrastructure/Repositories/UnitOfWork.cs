using Javeriana.Core.Contexts;
using Javeriana.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareasAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TareasContext _dbContext;

        public UnitOfWork(TareasContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Confirmar()
        {
            _dbContext.SaveChanges();
        }

        public async Task ConfirmarAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
