using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoRepositorio _prestamoRepositorio;

        public PrestamosController(PrestamoRepositorio prestamoRepositorio)
        {
            _prestamoRepositorio = prestamoRepositorio;
        }

        // GET: /Prestamos/Reporte?desde=2026-09-01&hasta=2026-09-30
        public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
        {
            // Si no se envía rango, se usan los últimos 30 días
            var fechaHasta = hasta ?? DateTime.Today;
            var fechaDesde = desde ?? fechaHasta.AddDays(-30);

            if (fechaDesde > fechaHasta)
            {
                ModelState.AddModelError(string.Empty, "La fecha 'Desde' no puede ser mayor que la fecha 'Hasta'.");
                ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
                ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");
                return View(Enumerable.Empty<Models.PrestamoReporte>());
            }

            var reporte = await _prestamoRepositorio.ReportePorFechasAsync(fechaDesde, fechaHasta);

            // Se conserva el rango elegido para mostrarlo de nuevo en el formulario
            ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
            ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

            return View(reporte);
        }
    }
}