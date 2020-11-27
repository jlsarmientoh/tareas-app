using Javeriana.Core.Seguridad.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Javeriana.Core.Tareas.Entities
{
    public class Tarea
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public Usuario owner { get; set; }
    }
}
