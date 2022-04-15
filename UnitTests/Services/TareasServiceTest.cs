using Javeriana.Core.Interfaces.Messaging;
using Javeriana.Core.Interfaces;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Services;
using Moq;
using Moq.Protected;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class TareasServiceTest
    {
        private readonly IAsyncRepository<Javeriana.Core.Tareas.Entities.Tarea> _repository;

        private readonly IPublisher _publisher;

        private readonly IUnitOfWork _unitOfWork;

        private readonly ITareasService _tareasService;

        public TareasServiceTest()
        {
            var mockRepository = new Mock<IAsyncRepository<Javeriana.Core.Tareas.Entities.Tarea>>();

            mockRepository.Setup(x => x.GetByIdAsync(It.Is<long>(id => id == 1)))
                .Returns(Task.FromResult(new Javeriana.Core.Tareas.Entities.Tarea
                {
                    Id = 1,
                    IsComplete = false,
                    Name = "Tarea 1"
                }
                ));

            mockRepository.Setup(x => x.GetByIdAsync(It.Is<long>(id => id == 2)))
                .Returns(Task.FromResult(new Javeriana.Core.Tareas.Entities.Tarea
                {
                    Id = 2,
                    IsComplete = true,
                    Name = "Tarea Modificada"
                }
                ));

            mockRepository.Setup(x => x.GetByIdAsync(It.Is<long>(id => id == 3)))
                .Returns(Task.FromResult(new Javeriana.Core.Tareas.Entities.Tarea()));

            /*
            mockRepository.Setup(x => x.ListAllAsync())
                .Returns(Task.FromResult(new List<Javeriana.Core.Tareas.Entities.Tarea>()
                    {
                        new Javeriana.Core.Tareas.Entities.Tarea{ Id=1, Name = "Tarea 1", IsComplete = false},
                        new Javeriana.Core.Tareas.Entities.Tarea{ Id=2, Name = "Tarea 2", IsComplete = false},
                        new Javeriana.Core.Tareas.Entities.Tarea{ Id=3, Name = "Tarea 3", IsComplete = false}
                    }
                ));
            */

            mockRepository.Setup(x => x.AddAsync(It.IsAny<Javeriana.Core.Tareas.Entities.Tarea>()))
                .Returns(Task.FromResult(new Javeriana.Core.Tareas.Entities.Tarea
                {
                    Id = 1,
                    Name = "Tarea 1",
                    IsComplete = false
                }
                ));

            mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Javeriana.Core.Tareas.Entities.Tarea>()));

            mockRepository.Setup(x => x.DeleteAsync(It.IsAny<Javeriana.Core.Tareas.Entities.Tarea>()));

            _repository = mockRepository.Object;


            _publisher = new Mock<IPublisher>().Object;
            _unitOfWork = new Mock<IUnitOfWork>().Object;

            _tareasService = new TareasServices(_repository, _unitOfWork, _publisher);
        }
        public async Task shouldGetListOfTareas()
        {
            var tareas = await _tareasService.GetTareasAsync();
            Assert.NotNull(tareas);
            //Assert.Equal(3, tareas.Count);
        }

        [Fact]
        public async Task shouldGetTarea()
        {
            var tarea = await _tareasService.GetTareaAsync(1);
            Assert.NotNull(tarea);
            Assert.True(tarea.Id == 1);
            Assert.Equal("Tarea 1", tarea.Name);
            Assert.False(tarea.IsComplete);
        }

        [Fact]
        public async Task shouldCreateTarea()
        {
            var tarea = new Javeriana.Api.DTO.Tarea { Name = "Tarea 1", IsComplete = false };
            var nuevaTarea = await _tareasService.CreateTareaAsync(tarea);
            Assert.NotNull(nuevaTarea);
            Assert.True(nuevaTarea.Id == 1);
            Assert.Equal(tarea.Name, nuevaTarea.Name);
            Assert.False(nuevaTarea.IsComplete);
        }

        [Fact]
        public async Task shouldUpdateTarea()
        {
            var tarea = new Javeriana.Api.DTO.Tarea { Id = 2, Name = "Tarea Modificada", IsComplete = true };
            await _tareasService.UpdateTareaAsync(tarea.Id, tarea);

            var tareaActualizada = await _tareasService.GetTareaAsync(2);
            Assert.NotNull(tareaActualizada);
            Assert.True(tareaActualizada.Id == 2);
            Assert.Equal("Tarea Modificada", tareaActualizada.Name);
            Assert.True(tareaActualizada.IsComplete);
        }

        [Fact]
        public async Task shouldDeleteTarea()
        {
            await _tareasService.DeleteTareaAsync(3);
            var tarea = await _tareasService.GetTareaAsync(3);
            Assert.NotNull(tarea);
            Assert.True(tarea.Id == 0);
            Assert.Equal(null, tarea.Name);
        }   
    }
}