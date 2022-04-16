using System.Threading.Tasks;
using Javeriana.Core.Tareas.Entities;
using TareasAPI.Repositories;
using Xunit;

namespace IntegrationTest
{
    [Collection("Sequential")]
    public class TareasRepositoryTest : IClassFixture<DatabaseFixture>
    {

        private readonly TareasRespository _repository;

        public TareasRepositoryTest(DatabaseFixture fixture)
        {
            _repository = new TareasRespository(fixture.Context);
        }

        [Fact]
        public async Task GetAllTareas()
        {
            // Act
            var tareas = await _repository.ListAllAsync();
            // Assert
            Assert.NotNull(tareas);
            Assert.True(tareas.Count > 0);
        }

        [Fact]
        public async Task GetSingleTarea()
        {
            // Act
            var tarea = await _repository.GetByIdAsync(1);
            // Assert
            Assert.NotNull(tarea);
            Assert.Equal(1, tarea.Id);
        }

        [Fact]
        public async Task UpdateTarea()
        {
            // Arrange
            var tarea = await _repository.GetByIdAsync(2);
            tarea.Name = "Modified Tarea 2";
            tarea.IsComplete = true;
            // Act
            await _repository.UpdateAsync(tarea);
            var tareaUpdated = await _repository.GetByIdAsync(2);
            // Assert
            Assert.Equal("Modified Tarea 2", tareaUpdated.Name);
            Assert.True(tareaUpdated.IsComplete);
        }

        [Fact]
        public async Task DeleteTarea()
        {
            // Arrange
            var nuevaTarea = new Tarea
            {
                Name = "Eliminar Tarea",
                IsComplete = false
            };
            // Act
            nuevaTarea = await _repository.AddAsync(nuevaTarea);
            var tarea = await _repository.GetByIdAsync(nuevaTarea.Id);
            await _repository.DeleteAsync(tarea);
            var tareaDeleted = await _repository.GetByIdAsync(nuevaTarea.Id);
            // Assert
            Assert.Null(tareaDeleted);
        }

        [Fact]
        public async Task AddTarea()
        {
            // Arrange
            var tarea = new Tarea
            {
                Name = "Crear Tarea",
                IsComplete = false
            };
            // Act
            tarea = await _repository.AddAsync(tarea);
            var tareaAdded = await _repository.GetByIdAsync(tarea.Id);
            // Assert
            Assert.NotNull(tareaAdded);
            Assert.Equal("Crear Tarea", tareaAdded.Name);
            Assert.False(tareaAdded.IsComplete);
        }
        
    }

}