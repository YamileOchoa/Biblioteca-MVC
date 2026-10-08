using System.Data;
using Biblioteca.Web.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Biblioteca.Web.Repositorios
{
    public class SocioRepositorio
    {
        private readonly string _cadenaConexion;

        public SocioRepositorio(IConfiguration configuration)
        {
            _cadenaConexion = configuration.GetConnectionString("BibliotecaDB")
                ?? throw new InvalidOperationException(
                    "Falta la cadena de conexión 'BibliotecaDB' en appsettings.json.");
        }

        public async Task<IEnumerable<Socio>> ListarAsync()
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            return await conexion.QueryAsync<Socio>(
                "usp_Socios_Listar",
                commandType: CommandType.StoredProcedure);
        }

        // Si el DNI ya existe, el procedimiento lanza SqlException con Number = 50001
        public async Task<int> InsertarAsync(Socio socio)
        {
            using var conexion = new SqlConnection(_cadenaConexion);
            var resultado = await conexion.QueryAsync<int>(
                "usp_Socios_Insertar",
                new
                {
                    socio.DNI,
                    socio.Nombre,
                    socio.Email
                },
                commandType: CommandType.StoredProcedure);
            return resultado.Single(); // NuevoId
        }
    }
}