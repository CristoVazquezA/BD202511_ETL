using CargaArchivos.DataBase;
using CargaArchivos.Entities;
using Microsoft.Data.SqlClient;

namespace CargaArchivos.Commands
{
    public class ProductoCommands
    {
        private readonly SQLServer _sqlServer;
        public ProductoCommands(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }
        public async Task<Producto> GetProductoAsync(int id)
        {
            try
            {
                string query = "SELECT * FROM Productos Where Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };

                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                return await _sqlServer.ReaderAsync<Producto>(query, parametros);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                string query = "SELECT * FROM Productos";
                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                List<Producto> productos = await _sqlServer.ReaderListAsync<Producto>(query);
                return productos;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> AddProductoAsync(Producto producto)
        {
            try
            {
                string query = "INSERT INTO Productos (SKU, Descripcion, ValorUnitario) " +
                               "VALUES (@SKU, @Descripcion, @ValorUnitario); " +
                               "SELECT SCOPE_IDENTITY();";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@SKU", producto.SKU),
                    new SqlParameter("@Descripcion", producto.Descripcion),
                    new SqlParameter("@ValorUnitario", producto.ValorUnitario)
                };
                int nuevoId = await _sqlServer.ScalarAsync<int>(query, parametros);
                return nuevoId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateProductoAsync(Producto producto)
        {
            try
            {
                string query = "UPDATE Productos SET SKU = @SKU, Descripcion = @Descripcion, ValorUnitario = @ValorUnitario " +
                               "WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", producto.Id),
                    new SqlParameter("@SKU", producto.SKU),
                    new SqlParameter("@Descripcion", producto.Descripcion),
                    new SqlParameter("@ValorUnitario", producto.ValorUnitario)
                };
                await _sqlServer.NonQueryAsync(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteProductoAsync(int id)
        {
            try
            {
                string query = "DELETE FROM Productos WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };
                await _sqlServer.NonQueryAsync(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
