using MongoDB.Driver;
using CargaArchivos.Dtos;


namespace CargaArchivos.DataBase
{
    public class MongoVentasRepository
    {
        private readonly IMongoCollection<VentaCompleta> _ventasCollection;

        public MongoVentasRepository(string connectionString, string databaseName = "Ventasdb")
        {
            var client = new MongoClient(connectionString);
            var db = client.GetDatabase(databaseName);

            _ventasCollection = db.GetCollection<VentaCompleta>("Ventas");
        }

        public async Task InsertarVentasAsync(List<VentaCompleta> ventas)
        {
            await _ventasCollection.InsertManyAsync(ventas);
        }
    }
}
