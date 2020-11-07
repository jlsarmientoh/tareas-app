using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Javeriana.Api.DTO;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Javeriana.Api.Controllers
{
    [ApiController]
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private ITareasService _servicio;

        public TareasController(ITareasService tareaService)
        {
            _servicio = tareaService;
        }

        [HttpGet]
        public IEnumerable<Tarea> GetTareas()
        {
            var tareas = _servicio.GetTareasAsync();
            return tareas.Result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Tarea> GetTarea(int id)
        {
            try{
                var tarea = _servicio.GetTarea(id);
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
        public ActionResult<Tarea> CreateTarea([FromBody]Tarea tarea)
        {
            if(!ModelState.IsValid) return BadRequest();

            var tareaCreada = _servicio.CreateTareaAsync(tarea);
            return Ok(tareaCreada);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateTarea(int id, [FromBody] Tarea tarea)
        {    
            try{
                if(!ModelState.IsValid) return BadRequest();

                _servicio.UpdateTareaAsync(id,tarea);
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
        public IActionResult DeleteTarea(int id){
            try
            {
                _servicio.DeleteTareaAsync(id);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}