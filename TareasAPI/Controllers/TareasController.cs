using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Javeriana.Api.DTO;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareasAsync()
        {
            _logger.LogDebug("List all tasks");
            try
            {
                var tareas = await _servicio.GetTareasAsync();
                _logger.LogInformation("TareasAPI.Metrics.TareasGet.OK");
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogInformation(ex, "TareasAPI.Metrics.TareasGet.Error");
                return Problem(ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            try{
                _logger.LogDebug("Find a given task by id {id}", id);
                var tarea = await _servicio.GetTareaAsync(id);
                _logger.LogInformation("TareasAPI.Metrics.TareasDetail.OK");
                return Ok(tarea);
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasDetail.ERROR");
                return NotFound(e.Message);
            } 
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasDetail.ERROR");
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Tarea>> CreateTareaAsync([FromBody]Tarea tarea)
        {
            try
            {
                if(!ModelState.IsValid) return BadRequest();

                _logger.LogDebug("Creating a new task");
                Tarea tareaCreada = await _servicio.CreateTareaAsync(tarea);
                _logger.LogInformation("TareasAPI.Metrics.TareasCreated.OK");
                return Ok(tareaCreada);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasCreated.ERROR");
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateTareaAsync(long id, [FromBody] Tarea tarea)
        {    
            try{
                if(!ModelState.IsValid) return BadRequest();

                _logger.LogDebug("Updating task with id {id}", id);
                await _servicio.UpdateTareaAsync(id,tarea);
                _logger.LogInformation("TareasAPI.Metrics.TareasUpdeted.OK");
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasUpdated.ERROR");
                return NotFound(tarea);
            }
            catch (Exception e){
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasUpdated.ERROR");
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteTareaAsync(int id){
            try
            {
                _logger.LogTrace("Deleting task {id}", id);
                await _servicio.DeleteTareaAsync(id);
                _logger.LogInformation("TareasAPI.Metrics.TareasDeleted.OK");
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasDeleted.ERROR");
                return NotFound(e.Message);
            }
            catch (Exception e){
                _logger.LogError(e.Message);
                _logger.LogInformation("TareasAPI.Metrics.TareasDeleted.ERROR");
                return Problem(e.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}