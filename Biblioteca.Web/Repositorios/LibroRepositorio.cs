using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios
{
    public class LibroRepositorio
    {
        private readonly string _cadenaConexion;

        public LibroRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException(
                    "Falta la cadena de conexión 'BibliotecaDB' en appsettings.json.");
        }

        public async Task<IEnumerable<Libro>> ListarAsync()
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            return await conexion.QueryAsync<Libro>(
                "usp_Libros_Listar",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Libro>> BuscarPorTituloAsync(string titulo)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            return await conexion.QueryAsync<Libro>(
                "usp_Libros_BuscarPorTitulo",
                new { Titulo = titulo },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<Libro?> ObtenerPorIdAsync(int libroId)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            var resultado = await conexion.QueryAsync<Libro>(
                "usp_Libros_ObtenerPorId",
                new { LibroId = libroId },
                commandType: CommandType.StoredProcedure);
            return resultado.FirstOrDefault();
        }

        public async Task<int> InsertarAsync(Libro libro)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            var resultado = await conexion.QueryAsync<int>(
                "usp_Libros_Insertar",
                new
                {
                    libro.Titulo,
                    libro.ISBN,
                    libro.AutorId,
                    libro.Ejemplares
                },
                commandType: CommandType.StoredProcedure);
            return resultado.Single(); // NuevoId
        }

        public async Task ActualizarAsync(Libro libro)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            await conexion.ExecuteAsync(
                "usp_Libros_Actualizar",
                new
                {
                    libro.LibroId,
                    libro.Titulo,
                    libro.ISBN,
                    libro.AutorId,
                    libro.Ejemplares
                },
                commandType: CommandType.StoredProcedure);
        }

        // Eliminación lógica (el procedimiento hace Activo = 0)
        public async Task EliminarAsync(int libroId)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            await conexion.ExecuteAsync(
                "usp_Libros_Eliminar",
                new { LibroId = libroId },
                commandType: CommandType.StoredProcedure);
        }

        // Para la lista desplegable de autores
        public async Task<IEnumerable<Autor>> ListarAutoresActivosAsync()
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            return await conexion.QueryAsync<Autor>(
                "usp_Autores_ListarActivos",
                commandType: CommandType.StoredProcedure);
        }
    }
}