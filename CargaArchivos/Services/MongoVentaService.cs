using MongoDB.Driver;
using CargaArchivos.Entities;

namespace CargaArchivos.Services

{
    public class MongoVentaService
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _config;

        public MongoVentaService(IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(config["MongoDB:ConnectionString"]);
            _database = client.GetDatabase(config["MongoDB:Database"]);
        }

        private IMongoCollection<VentaCompleta> GetCollection(string key)
        {
            string collectionName = _config[$"MongoDB:Collections:{key}"];

            if (string.IsNullOrEmpty(collectionName))
                throw new Exception($"No existe la colección definida en appsettings para la clave {key}");

            return _database.GetCollection<VentaCompleta>(collectionName);
        }

        public async Task InsertManyAsync(string tipo, List<VentaCompleta> ventas)
        {
            var collection = GetCollection(tipo);
            await collection.InsertManyAsync(ventas);
        }
    }
}
