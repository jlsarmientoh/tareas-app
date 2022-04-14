using System.Threading.Tasks;
using Javeriana.Core.Contexts;
using Javeriana.Core.Tareas.Entities;
using TareasAPI.Repositories;
using Xunit;

namespace IntegrationTest
{
    [Collection("Sequential")]
    public class UnitOfWorkTest : IClassFixture<DatabaseFixture>
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly TareasContext _dbContext;

        public UnitOfWorkTest(DatabaseFixture fixture)
        {
            _unitOfWork = new UnitOfWork(fixture.Context);
            _dbContext = fixture.Context;
        }

        [Fact]
        public void Confirmar()
        {
            Tarea tarea = new Tarea
            {
                Name = "Crear Tarea Unit of Work Sync",
                IsComplete = false
            };
            _dbContext.Tareas.Add(tarea);
            _unitOfWork.Confirmar();
            Assert.NotNull(tarea.Id);
            Assert.True(tarea.Id > 0);
        }

        [Fact]
        public async Task ConfirmarAsync()
        {
            Tarea tarea = new Tarea
            {
                Name = "Crear Tarea Unit of Work Async",
                IsComplete = false
            };
            _dbContext.Tareas.Add(tarea);
            await _unitOfWork.ConfirmarAsync();
            Assert.NotNull(tarea.Id);
            Assert.True(tarea.Id > 0);
        }

    }
}