using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios
{
    public class PrestamoRepositorio
    {
        private readonly string _cadenaConexion;

        public PrestamoRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException(
                    "Falta la cadena de conexión 'BibliotecaDB' en appsettings.json.");
        }

        public async Task<IEnumerable<PrestamoReporte>> ReportePorFechasAsync(DateTime desde, DateTime hasta)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            return await conexion.QueryAsync<PrestamoReporte>(
                "usp_Prestamos_ReportePorFechas",
                new { Desde = desde.Date, Hasta = hasta.Date },
                commandType: CommandType.StoredProcedure);
        }
    }
}