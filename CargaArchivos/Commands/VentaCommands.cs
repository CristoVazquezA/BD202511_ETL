using CargaArchivos.DataBase;
using CargaArchivos.Entities;
using Microsoft.Data.SqlClient;

namespace CargaArchivos.Commands
{
    public class VentaCommands
    {
        private readonly SQLServer _sqlServer;
        public VentaCommands(SQLServer sqlServer)
        {
            _sqlServer = sqlServer;
        }
        public async Task<Venta> GetVentaAsync(int id, bool incluirConceptos = false)
        {
            try
            {
                string query = "SELECT * FROM Ventas WHERE Id = @Id";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Id", id)
                };

                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                var venta = await _sqlServer.ReaderAsync<Venta>(query, parametros);
                if (venta != null && incluirConceptos)
                {
                    // Obtenemos los detalles de la venta
                    VentaDetalleCommands detalleCommands = new VentaDetalleCommands();
                    venta.Conceptos = await detalleCommands.GetVentasDetalleAsync(venta.Id);
                }
                return venta;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Venta>> GetVentasAsync()
        {
            try
            {
                string query = "SELECT * FROM Ventas";
                // Utilizamos la clase SQLServer para ejecutar la consulta y obtener el resultado
                List<Venta> ventas = await _sqlServer.ReaderListAsync<Venta>(query);
                return ventas;
            }
            catch (Exception)
            {

                throw;
            }

        }
        private async Task<int> AddVentaAsync(Venta venta)
        {
            try
            {
                string query = "INSERT INTO Ventas (Fecha, Folio, Total, ClienteId) " +
                               "VALUES (GetDate(), @Folio, @Total, @ClienteId); " +
                               "SELECT CAST (SCOPE_IDENTITY() AS INT);";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Folio", venta.Folio),
                    new SqlParameter("@Total", venta.Total),
                    new SqlParameter("@ClienteId", venta.ClienteID)
                };
                int nuevoId = await _sqlServer.ScalarAsync<int>(query, parametros);
                return nuevoId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<int> AddVentaTransactionAsync(SqlConnection sqlConnection, SqlTransaction sqlTransaction, Venta venta)
        {
            try
            {
                string query = "INSERT INTO Ventas (Fecha, Folio, Total, ClienteId) " +
                               "VALUES (GetDate(), @Folio, @Total, @ClienteId); " +
                               "SELECT CAST (SCOPE_IDENTITY() AS INT);";
                SqlParameter[] parametros = new SqlParameter[]
                {
                    new SqlParameter("@Folio", venta.Folio),
                    new SqlParameter("@Total", venta.Total),
                    new SqlParameter("@ClienteId", venta.ClienteID)
                };
                int nuevoId = await _sqlServer.ScalarAsync<int>(sqlConnection, sqlTransaction, query, parametros);
                return nuevoId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task SaveVentaAsync(Venta venta)
        {
            try
            {
                int ventaId = await AddVentaAsync(venta);

                foreach (var concepto in venta.Conceptos)
                {
                    VentaDetalleCommands detalleCommands = new VentaDetalleCommands();
                    await detalleCommands.AddVentaDetalleAsync(concepto, ventaId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task SaveVentaTransactionAsync(Venta venta)
        {

            using SqlConnection sqlConnection = _sqlServer.GetConnection();
            using SqlTransaction sqlTransaction = await _sqlServer.GetTransaction(sqlConnection);
            try
            {
                int ventaId = await AddVentaTransactionAsync(sqlConnection, sqlTransaction, venta);

                foreach (var concepto in venta.Conceptos)
                {
                    VentaDetalleCommands detalleCommands = new VentaDetalleCommands();
                    await detalleCommands.AddVentaDetalleTransactionAsync(sqlConnection, sqlTransaction, concepto, ventaId);
                }
                sqlTransaction.Commit();
            }
            catch (Exception)
            {
                sqlTransaction.Rollback();

                throw;
            }
        }
    }
}
