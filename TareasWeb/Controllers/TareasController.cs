using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.DTO;
using ApplicationCore.Interfaces;
using Javeriana.Api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TareasWeb.Controllers
{
    public class TareasController : Controller
    {
        private readonly IRestClient<Tarea> _restClient;

        private readonly ILogger<TareasController> _logger;

        private readonly IConfiguration _configuration;

        public TareasController(IRestClient<Tarea> restClient, ILogger<TareasController> logger, IConfiguration configuration)
        {
            _restClient = restClient;
            _logger = logger;
            _configuration = configuration;
        }


        // GET: TareasController
        public async Task<ActionResult> IndexAsync()
        {
            _logger.LogInformation("Preparando lista de tareas");
            Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Lista"));
            Respuesta<IEnumerable<Tarea>> respuesta = await _restClient.GetAsync<IEnumerable<Tarea>>(peticion);
            _logger.LogInformation("Lista de tareas parseadas, enviando a la vista");
            return View(respuesta.Body);
        }

        // GET: TareasController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            
            return View( await getTareaDetails(id));
        }

        // GET: TareasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TareasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Name")] Tarea nuevaTarea )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _logger.LogInformation($"Enviando nueva tareas al servicio");
                    Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Nueva"))
                    {
                        Body = nuevaTarea
                    };
                    Respuesta<Tarea> respuesta = await _restClient.PostAsync<Tarea>(peticion);
                    _logger.LogInformation($"La tarea ha sido creada");
                }

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"No se pudo crear la tarea : ${ex.Message}");
                return View();
            }
        }

        // GET: TareasController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            return View(await getTareaDetails(id));
        }

        // POST: TareasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Name,IsComplete")] Tarea tarea)
        {
            try
            {
                Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Editar"))
                {
                    Body = tarea
                };
                peticion.PathVariables.Add(tarea.Id.ToString());
                Respuesta<Tarea> respuesta = await _restClient.PutAsync<Tarea>(peticion);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TareasController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await getTareaDetails(id));
        }

        // POST: TareasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Eliminar"));
                peticion.PathVariables.Add(id.ToString());
                Respuesta<Tarea> respuesta = await _restClient.DeleteAsync<Tarea>(peticion);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private async Task<Tarea> getTareaDetails(int id)
        {
            Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Detalle"));
            peticion.PathVariables.Add(id.ToString());
            Respuesta<Tarea> respuesta = await _restClient.GetAsync<Tarea>(peticion);
            return respuesta.Body;
        }
    }
}
