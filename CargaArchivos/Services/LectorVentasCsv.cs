
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CargaArchivos.Dtos;

namespace CargaArchivos.Services
{
    public class LectorVentasCsv
    {
        public static List<VentaCompleta> LeerVentasDesdeCsv()
        {
            string rutaArchivoCsv = @"C:\Users\cj_13\source\repos\BD202511_ETL\ArchivosParaProcesar\ventas_bigdata_1000.csv";

            var lista = new List<VentaCompleta>();

            if (!File.Exists(rutaArchivoCsv))
                throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivoCsv}");

            var lineas = File.ReadAllLines(rutaArchivoCsv);

            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                var c = linea.Split(',');

                if (c.Length < 14)
                    throw new Exception($"Formato inválido en línea: {linea}");

                lista.Add(new VentaCompleta
                {
                    VentaId = int.Parse(c[0]),
                    Fecha = DateTime.Parse(c[1]),
                    Folio = int.Parse(c[2]),

                    ClienteId = int.Parse(c[3]),
                    NombreCliente = c[4],
                    Telefono = c[5],
                    Domicilio = c[6],

                    ProductoId = int.Parse(c[7]),
                    SKU = c[8],
                    DescripcionProducto = c[9],

                    Cantidad = int.Parse(c[10]),
                    ValorUnitario = decimal.Parse(c[11], CultureInfo.InvariantCulture),
                    Importe = decimal.Parse(c[12], CultureInfo.InvariantCulture),
                    TotalVenta = decimal.Parse(c[13], CultureInfo.InvariantCulture)
                });
            }

            return lista;
        }

        internal static List<VentaCompleta> LeerVentasCsv()
        {
            throw new NotImplementedException();
        }
    }
}
