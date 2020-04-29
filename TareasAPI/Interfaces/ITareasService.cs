using System;
using System.Collections.Generic;
using Javeriana.Api.Model;

namespace Javeriana.Api.Interfaces
{
    public interface ITareasService
    {
        public IEnumerable<Tarea> GetTareas();

        public Tarea GetTarea(long Id);

        public Tarea CreateTarea(Tarea tarea);

        public void UpdateTarea(long id, Tarea tareaActualizada);

        public void DeleteTarea(long id);
    }
}