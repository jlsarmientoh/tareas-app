using Javeriana.Api.DTO;
using ApplicationCore.DTO;
using Xunit;

namespace UnitTests
{
    public class DTOTest
    {
        [Fact]
        public void shoulPopulatePeticionDTO()
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>("localhost:8080/api/tareas/{0}"){
                Body = new Tarea{
                    Id = 1,
                    Name = "Tarea 1",
                    IsComplete = false
                }
            };

            peticion.PathVariables.Add("1");
            var url = peticion.ResolverRequestURL();

            Assert.Equal("localhost:8080/api/tareas/1", url);
            Assert.NotNull(peticion.Body);
            Assert.NotNull(peticion.PathVariables);
            Assert.NotNull(peticion.Headers);
        }

        [Fact]
        public void shouldPopulateTareaDTO()
        {
            Tarea tarea = new Tarea{
                Id = 1,
                Name = "Tarea 1",
                IsComplete = false
            };

            Assert.NotNull(tarea);
            Assert.Equal(1, tarea.Id);
            Assert.Equal("Tarea 1", tarea.Name);
            Assert.False(tarea.IsComplete);
        }
    }
}