using Javeriana.Core.Seguridad.Entities;

namespace Javeriana.Core.Tareas.Entities
{
    public class Tarea
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsComplete { get; set; }

        public Usuario Owner { get; set; }
    }
}
