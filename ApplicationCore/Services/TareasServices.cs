using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Javeriana.Api.Exceptions;
using Javeriana.Api.Interfaces;
using Javeriana.Api.DTO;
using Javeriana.Core.Interfaces;
using Javeriana.Core.Interfaces.Messaging;
using ApplicationCore.DTO;
using System;

namespace Javeriana.Api.Services
{
    public class TareasServices : ITareasService
    {
        private IAsyncRepository<Javeriana.Core.Tareas.Entities.Tarea> _respository;

        private IUnitOfWork _unitOfWork;

        private IPublisher _publisher;

        public TareasServices(IAsyncRepository<Javeriana.Core.Tareas.Entities.Tarea> respository, IUnitOfWork unitOfWork, IPublisher publisher)
        {
            _respository = respository;
            _unitOfWork = unitOfWork;
            _publisher = publisher;
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

        public async Task<Tarea> GetTareaAsync(long Id)
        {
            var resultado = await _respository.GetByIdAsync(Id);

            if(resultado == null) throw new TareaNoExisteException();

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
            var nuevaTarea = new Core.Tareas.Entities.Tarea
            { 
                IsComplete = tarea.IsComplete,
                Name = tarea.Name
            };
            nuevaTarea = await _respository.AddAsync(nuevaTarea);
            await _unitOfWork.ConfirmarAsync();
            tarea.Id = nuevaTarea.Id;

            Mensaje mensaje = new Mensaje
            {
                FechaEnvio = DateTime.Now,
                Tarea = tarea
            };

            
            //_publisher.PublicarMensaje(mensaje);
            //_publisher.DistribuirMensaje(mensaje);

            return tarea;
        }

        public async Task UpdateTareaAsync(long id, Tarea tareaActualizada){
            var tareaOrigen = await _respository.GetByIdAsync(id);
             if(tareaOrigen == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);
             
            tareaOrigen.Name = tareaActualizada.Name;
            tareaOrigen.IsComplete = tareaActualizada.IsComplete;

            await _respository.UpdateAsync(tareaOrigen);
        }

        public async Task DeleteTareaAsync(long id)
        {
            var tareaElminar = await _respository.GetByIdAsync(id);
            if (tareaElminar == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + id);

            await _respository.DeleteAsync(tareaElminar);
            await _unitOfWork.ConfirmarAsync();
        }
    }
}