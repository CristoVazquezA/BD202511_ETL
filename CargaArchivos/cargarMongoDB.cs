using CargaArchivos.DataBase;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargaArchivos.Dtos;
using CargaArchivos.Services;

namespace CargaArchivos
{
    public class cargarMongoDB
    {
        static async Task Main(string[] args)
        {
            // 1. Leer CSV
            var listaVentas = LectorVentasCsv.LeerVentasCsv();

            // 2. Conectar Mongo
            var repo = new MongoVentasRepository(
                "mongodb://localhost:27017", // Cambia si usas Atlas u otra URL
                "VentasDB"
            );

            // 3. Insertar en MongoDB
            await repo.InsertarVentasAsync(listaVentas);

            Console.WriteLine($"Se insertaron {listaVentas.Count} registros.");
        }
    }
}
