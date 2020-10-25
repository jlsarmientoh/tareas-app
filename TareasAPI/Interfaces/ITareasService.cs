using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javeriana.Api.Model;

namespace Javeriana.Api.Interfaces
{
    public interface ITareasService
    {
        public Task<IEnumerable<Tarea>> GetTareasAsync();

        public Task<Tarea> GetTarea(int Id);

        public Task<Tarea> CreateTareaAsync(Tarea tarea);

        public Task UpdateTareaAsync(int id, Tarea tareaActualizada);

        public Task DeleteTareaAsync(int id);
    }
}