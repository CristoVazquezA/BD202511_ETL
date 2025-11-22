using CargaArchivos.DataBase;
using CargaArchivos.Entities;
using Microsoft.Data.SqlClient;

namespace CargaArchivos.Commands
{
    public class VentaDetalleCommands
    {
        private readonly SQLServer _sqlServer;
        public VentaDetalleCommands(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }
        public async Task<List<VentaDetalle>> GetVentasDetalleAsync(int ventaId)
        {
            try
            {
                string query = "SELECT * FROM VentasDetalle WHERE VentaId = @VentaId";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@VentaId", ventaId)
                };

                List<VentaDetalle> conceptos = await _sqlServer.ReaderListAsync<VentaDetalle>(query, parametros);
                return conceptos;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task AddVentaDetalleAsync(VentaDetalle concepto, int ventaId)
        {
            try
            {
                string query = "INSERT INTO VentaDetalle " +
                    "(VentaId, Renglon, ProductoId, Cantidad, ValorUnitario, Descripcion, Importe) " +
                               "VALUES " +
                    "(@VentaId, @Renglon, @ProductoId, @Cantidad, @ValorUnitario, @Descripcion, @Importe);";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@VentaId", ventaId),
                    new SqlParameter("@Renglon", concepto.Renglon),
                    new SqlParameter("@ProductoId", concepto.ProductoId),
                    new SqlParameter("@Cantidad", concepto.Cantidad),
                    new SqlParameter("@ValorUnitario", concepto.ValorUnitario),
                    new SqlParameter("@Descripcion", concepto.Descripcion),
                    new SqlParameter("@Importe", concepto.Importe)
                };
                await _sqlServer.NonQueryAsync(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task AddVentaDetalleTransactionAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, VentaDetalle concepto, int ventaId)
        {
            try
            {
                string query = "INSERT INTO VentaDetalle " +
                    "(VentaId, Renglon, ProductoId, Cantidad, ValorUnitario, Descripcion, Importe) " +
                               "VALUES " +
                    "(@VentaId, @Renglon, @ProductoId, @Cantidad, @ValorUnitario, @Descripcion, @Importe);";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@VentaId", ventaId),
                    new SqlParameter("@Renglon", concepto.Renglon),
                    new SqlParameter("@ProductoId", concepto.ProductoId),
                    new SqlParameter("@Cantidad", concepto.Cantidad),
                    new SqlParameter("@ValorUnitario", concepto.ValorUnitario),
                    new SqlParameter("@Descripcion", concepto.Descripcion),
                    new SqlParameter("@Importe", concepto.Importe)
                };
                await _sqlServer.NonQueryAsync(sqlConnection, sqlTransaction, query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
