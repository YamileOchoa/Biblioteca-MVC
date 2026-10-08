using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers
{
    public class SociosController : Controller
    {
        private readonly SocioRepositorio _socioRepositorio;

        public SociosController(SocioRepositorio socioRepositorio)
        {
            _socioRepositorio = socioRepositorio;
        }

        // GET: /Socios
        public async Task<IActionResult> Index()
        {
            var socios = await _socioRepositorio.ListarAsync();
            return View(socios);
        }

        // GET: /Socios/Create
        public IActionResult Create()
        {
            return View(new Socio());
        }

        // POST: /Socios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Socio socio)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _socioRepositorio.InsertarAsync(socio);
                    TempData["Mensaje"] = $"El socio \"{socio.Nombre}\" se registró correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex) when (ex.Number == 50001)
                {
                    // Error lanzado por THROW 50001 en usp_Socios_Insertar
                    ModelState.AddModelError(nameof(Socio.DNI), "Ya existe un socio con ese DNI.");
                }
            }

            return View(socio);
        }
    }
}