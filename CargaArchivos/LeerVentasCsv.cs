namespace CargaArchivos
{
    using CargaArchivos.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    public class LeerVentasCsv
    {
       
        private readonly string rutaArchivoCsv = @"C:\Users\cj_13\source\repos\BD202511_ETL\ArchivosParaProcesar\ventas_bigdata_1000.csv";

        public List<VentaCompleta> Leer()
        {
            var lista = new List<VentaCompleta>();

            if (!File.Exists(rutaArchivoCsv))
                throw new FileNotFoundException($"No se encontró el archivo: {rutaArchivoCsv}");

            var lineas = File.ReadAllLines(rutaArchivoCsv);

            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea))
                    continue;

                var columnas = linea.Split(',');

                if (columnas.Length < 14)
                    throw new Exception($"Formato inválido en línea: {linea}");

                var venta = new VentaCompleta
                {
                    VentaId = int.Parse(columnas[0]),
                    Fecha = DateTime.Parse(columnas[1]),
                    Folio = int.Parse(columnas[2]),
                    ClienteId = int.Parse(columnas[3]),
                    NombreCliente = columnas[4],
                    Telefono = columnas[5],
                    Domicilio = columnas[6],
                    ProductoId = int.Parse(columnas[7]),
                    SKU = columnas[8],
                    DescripcionProducto = columnas[9],
                    Cantidad = int.Parse(columnas[10]),
                    ValorUnitario = decimal.Parse(columnas[11], CultureInfo.InvariantCulture),
                    Importe = decimal.Parse(columnas[12], CultureInfo.InvariantCulture),
                    TotalVenta = decimal.Parse(columnas[13], CultureInfo.InvariantCulture)
                };

                lista.Add(venta);
            }

            return lista;
        }
    }
}
