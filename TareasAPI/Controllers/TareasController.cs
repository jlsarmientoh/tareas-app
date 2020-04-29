using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Javeriana.Api.Model;
using Javeriana.Api.Interfaces;
using Javeriana.Api.Exceptions;

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
            var tareas = _servicio.GetTareas();
            return tareas;
        }

        [HttpGet("{id}")]
        public ActionResult<Tarea> GetTarea(long id)
        {
            try{
                var tarea = _servicio.GetTarea(id);
                return Ok(tarea);
            }
            catch (TareaNoExisteException e)
            {
                return NotFound();
            } 
        }

        [HttpPost]
        public ActionResult<Tarea> CreateTarea([FromBody]Tarea tarea)
        {
            if(!ModelState.IsValid) return BadRequest();

            var tareaCreada = _servicio.CreateTarea(tarea);
            return Ok(tareaCreada);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTarea(long id, [FromBody] Tarea tarea)
        {    
            try{
                _servicio.UpdateTarea(id,tarea);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                return NotFound(tarea);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTarea(long id){
            try
            {
                _servicio.DeleteTarea(id);
                return Ok();
            }
            catch (TareaNoExisteException e)
            {
                return NotFound();
            }
        }
    }
}