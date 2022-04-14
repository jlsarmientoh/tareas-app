using System.Threading.Tasks;
using Javeriana.Core.Tareas.Entities;
using TareasAPI.Repositories;
using Xunit;

namespace IntegrationTest
{

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
            var tareas = await _repository.ListAllAsync();
            Assert.NotNull(tareas);
            Assert.True(tareas.Count > 0);
        }

        [Fact]
        public async Task GetSingleTarea()
        {
            var tarea = await _repository.GetByIdAsync(1);
            Assert.NotNull(tarea);
            Assert.Equal(1, tarea.Id);
        }

        [Fact]
        public async Task UpdateTarea()
        {
            var tarea = await _repository.GetByIdAsync(2);
            tarea.Name = "Modified Tarea 2";
            tarea.IsComplete = true;
            await _repository.UpdateAsync(tarea);
            var tareaUpdated = await _repository.GetByIdAsync(2);
            Assert.Equal("Modified Tarea 2", tareaUpdated.Name);
            Assert.Equal(true, tareaUpdated.IsComplete);
        }

        [Fact]
        public async Task DeleteTarea()
        {
            var nuevaTarea = new Tarea
            {
                Name = "Eliminar Tarea",
                IsComplete = false
            };
            nuevaTarea = await _repository.AddAsync(nuevaTarea);
            var tarea = await _repository.GetByIdAsync(nuevaTarea.Id);
            await _repository.DeleteAsync(tarea);
            var tareaDeleted = await _repository.GetByIdAsync(nuevaTarea.Id);
            Assert.Null(tareaDeleted);
        }

        [Fact]
        public async Task AddTarea()
        {
            var tarea = new Tarea
            {
                Name = "Crear Tarea",
                IsComplete = false
            };
            tarea = await _repository.AddAsync(tarea);
            var tareaAdded = await _repository.GetByIdAsync(tarea.Id);
            Assert.NotNull(tareaAdded);
            Assert.Equal("Crear Tarea", tareaAdded.Name);
            Assert.Equal(false, tareaAdded.IsComplete);
        }
        
    }

}