using System;
using Javeriana.Core.Contexts;
using Javeriana.Core.Seguridad.Entities;
using Javeriana.Core.Tareas.Entities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTest
{
    public class DatabaseFixture : IDisposable
    {
        private readonly TareasContext _context;
        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<TareasContext>()
                    .UseInMemoryDatabase(databaseName: "Tareas")
                    .Options;
                _context = new TareasContext(options);

            // ... initialize data in the test database ...

            Usuario usuario = new Usuario
            {
                UsuarioId = 1,
                Name = "Test User",
                Email = "test@test.com"
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges(); 

            Tarea tarea1 = new Tarea
            {
                Id = 1,
                Name = "Tarea 1",
                IsComplete = false,
                Owner = usuario
            };

            Tarea tarea2 = new Tarea
            {
                Id = 2,
                Name = "Tarea 2",
                IsComplete = false,
                Owner = usuario
            };

            Tarea tarea3 = new Tarea
            {
                Id = 3,
                Name = "Tarea 3",
                IsComplete = false,
                Owner = usuario
            };

            _context.Tareas.Add(tarea1);
            _context.Tareas.Add(tarea2);
            _context.Tareas.Add(tarea3);

            _context.SaveChanges();
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            Context.Database.EnsureDeleted();
        }

        public TareasContext Context => _context;

    }
}
