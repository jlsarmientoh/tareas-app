﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IRestClient<Tarea, IEnumerable<Tarea>> _restClient;

        private readonly ILogger<TareasController> _logger;

        private readonly IConfiguration _configuration;

        public TareasController(IRestClient<Tarea, IEnumerable<Tarea>> restClient, ILogger<TareasController> logger, IConfiguration configuration)
        {
            _restClient = restClient;
            _logger = logger;
            _configuration = configuration;
        }


        // GET: TareasController
        public async Task<ActionResult> IndexAsync()
        {
            _logger.LogInformation("Preparando lista de tareas");
            Peticion<Tarea> peticion = new Peticion<Tarea>(_configuration.GetValue<string>("Api:Tareas:Todas"));
            Respuesta<IEnumerable<Tarea>> respuesta = await _restClient.Get(peticion);
            _logger.LogInformation("Lista de tareas parseadas, enviando a la vista");
            return View(respuesta.Body);
        }

        // GET: TareasController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TareasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TareasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: TareasController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TareasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: TareasController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TareasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
