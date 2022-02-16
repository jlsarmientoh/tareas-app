using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Javeriana.Api.DTO;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Javeriana.Api.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ITareasService _servicio;

        private readonly ILogger<TareasController> _logger;

        public TareasController(ITareasService tareaService, ILogger<TareasController> log)
        {
            _servicio = tareaService;
            _logger = log;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            _logger.LogInformation("List all tasks");
            var tareas = await _servicio.GetTareasAsync();
            return tareas;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            try{
                _logger.LogInformation("Find a given task by id {id}", id);
                var tarea = await _servicio.GetTareaAsync(id);
                return Ok(tarea);
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            } 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Tarea>> CreateTareaAsync([FromBody]Tarea tarea)
        {
            if(!ModelState.IsValid) return BadRequest();

            _logger.LogInformation("Creating a new task");
            Tarea tareaCreada = await _servicio.CreateTareaAsync(tarea);
            return Ok(tareaCreada);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTareaAsync(long id, [FromBody] Tarea tarea)
        {    
            try{
                if(!ModelState.IsValid) return BadRequest();

                _logger.LogInformation("Updating task with id {id}", id);
                await _servicio.UpdateTareaAsync(id,tarea);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                return NotFound(tarea);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTareaAsync(int id){
            try
            {
                _logger.LogInformation("Deleting task {id}", id);
                await _servicio.DeleteTareaAsync(id);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                return NotFound(e.Message);
            }
        }
    }
}