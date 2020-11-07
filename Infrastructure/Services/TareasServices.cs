using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javeriana.Api.Exceptions;
using Javeriana.Api.Interfaces;
using Javeriana.Api.DTO;
using Javeriana.Core.Interfaces;

namespace Javeriana.Api.Services
{
    public class TareasServices : ITareasService
    {
        private IAsyncRepository<Javeriana.Core.Entities.Tarea> _respository;

        private IUnitOfWork _unitOfWork;

        public TareasServices(IAsyncRepository<Javeriana.Core.Entities.Tarea> respository, IUnitOfWork unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            var resultado = await _respository.ListAllAsync();

            var tareas = resultado
                .Select( tarea => new Tarea 
                { 
                    Id = tarea.Id, 
                    IsComplete = tarea.IsComplete , 
                    Name = tarea.Name 
                });

            return tareas;
        }

        public async Task<Tarea> GetTarea(int Id)
        {
            var resultado = await _respository.GetByIdAsync(Id);

            var tarea = new Tarea 
            {
                Id = resultado.Id,
                IsComplete = resultado.IsComplete,
                Name = resultado.Name 
            };

            return tarea;
        }

        public async Task<Tarea> CreateTareaAsync(Tarea tarea)
        {
            var nuevaTarea = new Core.Entities.Tarea
            { 
                IsComplete = tarea.IsComplete,
                Name = tarea.Name
            };
            nuevaTarea = await _respository.AddAsync(nuevaTarea);
            await _unitOfWork.ConfirmarAsync();
            tarea.Id = nuevaTarea.Id;
            return tarea;
        }

        public async Task UpdateTareaAsync(int id, Tarea tareaActualizada){
            var tareaOrigen = await _respository.GetByIdAsync(id);
             if(tareaOrigen == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);
             
            tareaOrigen.Name = tareaActualizada.Name;
            tareaOrigen.IsComplete = tareaActualizada.IsComplete;

            await _respository.UpdateAsync(tareaOrigen);
        }

        public async Task DeleteTareaAsync(int id)
        {
            var tareaElminar = await _respository.GetByIdAsync(id);
            if (tareaElminar == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);

            await _respository.DeleteAsync(tareaElminar);
        }
    }
}