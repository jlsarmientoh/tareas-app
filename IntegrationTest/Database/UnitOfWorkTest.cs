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
            // Arrange
            Tarea tarea = new Tarea
            {
                Name = "Crear Tarea Unit of Work Sync",
                IsComplete = false
            };
            // Act
            _dbContext.Tareas.Add(tarea);
            _unitOfWork.Confirmar();
            // Assert
            Assert.True(tarea.Id > 0);
        }

        [Fact]
        public async Task ConfirmarAsync()
        {
            // Arrange
            Tarea tarea = new Tarea
            {
                Name = "Crear Tarea Unit of Work Async",
                IsComplete = false
            };
            // Act
            _dbContext.Tareas.Add(tarea);
            await _unitOfWork.ConfirmarAsync();
            // Assert
            Assert.True(tarea.Id > 0);
        }

    }
}