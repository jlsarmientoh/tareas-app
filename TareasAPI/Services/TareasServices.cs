using System;
using System.Collections;
using System.Collections.Generic;
using Javeriana.Api.Exceptions;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Model;

namespace Javeriana.Api.Services
{
    public class TareasServices : ITareasService
    {
        private List<Tarea> misTareas;

        private int contador;

        public TareasServices()
        {
            this.contador = 1;
            this.misTareas = new List<Tarea>();
        }

        public IEnumerable<Tarea> GetTareas()
        {
            return misTareas;
        }

        public Tarea GetTarea(long Id)
        {
            var tarea = misTareas.Find(tarea => tarea.Id == Id);

            if(tarea == null) throw new TareaNoExisteException("La tarea con el siguiente id no existe: " + Id);

            return tarea;
        }

        public Tarea CreateTarea(Tarea tarea)
        {
            tarea.Id = contador;
            misTareas.Add(tarea);
            contador++;

            return tarea;
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