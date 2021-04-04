using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Javeriana.Api.DTO;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Javeriana.Api.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ITareasService _servicio;

        public TareasController(ITareasService tareaService)
        {
            _servicio = tareaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Tarea>> GetTareasAsync()
        {
            var tareas = await _servicio.GetTareasAsync();
            return tareas;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            try{
                var tarea = await _servicio.GetTareaAsync(id);
                return Ok(tarea);
            }
            catch (TareaNoExisteException e)
            {
                return NotFound(e.Message);
            } 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Tarea>> CreateTareaAsync([FromBody]Tarea tarea)
        {
            if(!ModelState.IsValid) return BadRequest();

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

                await _servicio.UpdateTareaAsync(id,tarea);
                return Ok();
            }
            catch (TareaNoExisteException)
            {
                return NotFound(tarea);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTareaAsync(int id){
            try
            {
                await _servicio.DeleteTareaAsync(id);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}