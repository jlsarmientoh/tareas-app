using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javeriana.Api.DTO;

namespace Javeriana.Api.Interfaces
{
    public interface ITareasService
    {
         Task<IEnumerable<Tarea>> GetTareasAsync();

        public Task<Tarea> GetTareaAsync(long Id);

        public Task<Tarea> CreateTareaAsync(Tarea tarea);

        public Task UpdateTareaAsync(long id, Tarea tareaActualizada);

        public Task DeleteTareaAsync(long id);
    }
}