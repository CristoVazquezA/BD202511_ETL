using MongoDB.Driver;
using CargaArchivos.Entities;

namespace CargaArchivos.Services

{
    public class MongoVentaService
    {
        private readonly IMongoCollection<VentaCompleta> _ventas;

        public MongoVentaService(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDB:Database"]);
            _ventas = database.GetCollection<VentaCompleta>(config["MongoDB:Collection"]);
        }

        public async Task InsertManyAsync(List<VentaCompleta> ventas)
        {
            await _ventas.InsertManyAsync(ventas);
        }
    }
}
