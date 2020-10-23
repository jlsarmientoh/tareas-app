using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javeriana.Api.Exceptions;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Model;
using Javeriana.Core.Interfaces;

namespace Javeriana.Api.Services
{
    public class TareasServices : ITareasService
    {
        private IAsyncRepository<Javeriana.Core.Entities.Tarea> _respository;

        private IUnitOfWork _unitOfWork;

        private List<Tarea> misTareas;

        private int contador;

        public TareasServices(IAsyncRepository<Javeriana.Core.Entities.Tarea> respository, IUnitOfWork unitOfWork)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            return await _respository.ListAllAsync();
        }

        public async Task<Tarea> GetTarea(int Id)
        {
            return await _respository.GetByIdAsync(Id);
        }

        public async Task<Tarea> CreateTareaAsync(Tarea tarea)
        {
            var tareaCreada = await _respository.AddAsync(tarea);
            await _unitOfWork.ConfirmarAsync();
            return tareaCreada;
        }

        public void UpdateTarea(long id, Tarea tareaActualizada){
             var tareaOrigen = misTareas.Find(tarea => tarea.Id == id);
             if(tareaOrigen == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);
             
            tareaOrigen.Name = tareaActualizada.Name;
            tareaOrigen.IsComplete = tareaActualizada.IsComplete;             
        }

        public void DeleteTarea(long id)
        {
            var tareaElminar = misTareas.Find(tarea => tarea.Id == id);
            if(tareaElminar == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);
            
            misTareas.Remove(tareaElminar);
        }
    }
}