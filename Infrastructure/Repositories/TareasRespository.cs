using Javeriana.Core.Contexts;
using Javeriana.Core.Tareas.Entities;
using Javeriana.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TareasAPI.Repositories
{
    public class TareasRespository : IAsyncRepository<Tarea>
    {
        protected readonly TareasContext _dbContext;

        public TareasRespository(TareasContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Tarea> AddAsync(Tarea entity)
        {
            await _dbContext.Tareas.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(Tarea entity)
        {
            _dbContext.Tareas.Remove(entity);
        }

        public async Task<Tarea> GetByIdAsync(int id)
        {
            return await _dbContext.Tareas.FindAsync(id);
        }

        public async Task<IReadOnlyList<Tarea>> ListAllAsync()
        {
            return await _dbContext.Tareas.ToListAsync();
        }

        public async Task UpdateAsync(Tarea entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            //await _dbContext.SaveChangesAsync();
        }
    }
}
