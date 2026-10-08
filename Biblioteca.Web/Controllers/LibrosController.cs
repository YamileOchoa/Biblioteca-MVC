using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibroRepositorio _libroRepositorio;

        public LibrosController(LibroRepositorio libroRepositorio)
        {
            _libroRepositorio = libroRepositorio;
        }

        // GET: /Libros?titulo=...
        public async Task<IActionResult> Index(string? titulo)
        {
            var libros = string.IsNullOrWhiteSpace(titulo)
                ? await _libroRepositorio.ListarAsync()
                : await _libroRepositorio.BuscarPorTituloAsync(titulo.Trim());

            ViewData["Busqueda"] = titulo;
            return View(libros);
        }

        // GET: /Libros/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // GET: /Libros/Create
        public async Task<IActionResult> Create()
        {
            await CargarAutoresAsync();
            return View(new Libro { Ejemplares = 1 });
        }

        // POST: /Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _libroRepositorio.InsertarAsync(libro);
                    TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se registró correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    // Violación de UNIQUE (ISBN duplicado)
                    ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe un libro con ese ISBN.");
                }
            }

            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        // GET: /Libros/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        // POST: /Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Libro libro)
        {
            if (id != libro.LibroId) return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    await _libroRepositorio.ActualizarAsync(libro);
                    TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" se actualizó correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
                {
                    ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe otro libro con ese ISBN.");
                }
            }

            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        // GET: /Libros/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        // POST: /Libros/Delete/5  (eliminación lógica)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _libroRepositorio.ObtenerPorIdAsync(id);
            if (libro == null) return NotFound();

            await _libroRepositorio.EliminarAsync(id);
            TempData["Mensaje"] = $"El libro \"{libro.Titulo}\" fue eliminado.";
            return RedirectToAction(nameof(Index));
        }

        // Llena la lista desplegable de autores
        private async Task CargarAutoresAsync(int? autorSeleccionado = null)
        {
            var autores = await _libroRepositorio.ListarAutoresActivosAsync();
            ViewData["Autores"] = new SelectList(autores, "AutorId", "Nombre", autorSeleccionado);
        }
    }
}