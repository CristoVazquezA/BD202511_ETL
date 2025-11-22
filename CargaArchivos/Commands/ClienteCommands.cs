using CargaArchivos.DataBase;
using CargaArchivos.Entities;
using Microsoft.Data.SqlClient;

namespace CargaArchivos.Commands
{
    public class ClienteCommands
    {
        private readonly SQLServer _sqlServer;
        public ClienteCommands(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }
        public async Task<Cliente> GetClienteAsync(int id)
        {
            try
            {
                string query = "SELECT * FROM Clientes WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };

                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                return await _sqlServer.ReaderAsync<Cliente>(query, parametros);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                string query = "SELECT * FROM Clientes";
                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                List<Cliente> clientes = await _sqlServer.ReaderListAsync<Cliente>(query);
                return clientes;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<int> AddClienteAsync(Cliente cliente)
        {
            try
            {
                string query = "INSERT INTO Clientes (Nombre, Telefono, Domicilio) " +
                               "VALUES (@Nombre, @Telefono, @Domicilio); " +
                               "SELECT SCOPE_IDENTITY();";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Nombre", cliente.Nombre),
                    new SqlParameter("@Telefono", cliente.Telefono),
                    new SqlParameter("@Domicilio", cliente.Domicilio)
                };
                int nuevoId = await _sqlServer.ScalarAsync<int>(query, parametros);
                return nuevoId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cliente> UpdateClienteAsync(int id)
        {
            try
            {
                string query = "UPDATE Clientes SET Nombre = @Nombre, Telefono = @Telefono, Domicilio = @Domicilio " +
                               "WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Nombre", "Jorge"),
                    new SqlParameter("@Telefono", "987654321"),
                    new SqlParameter("@Domicilio", "Calle 456")
                };
                await _sqlServer.NonQueryAsync(query, parametros);
                return await GetClienteAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Cliente> DeleteClienteAsync(int id)
        {
            try
            {
                string query = "DELETE FROM Clientes WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };
                await _sqlServer.NonQueryAsync(query, parametros);
                return null; // Retornamos null ya que el cliente ha sido eliminado
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
