using CargaArchivos.Entities;
using CargaArchivos.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace CargaArchivos.Components
{
    public partial class CargarArchivo
    {
        [Inject]
        public MongoVentaService MongoService { get; set; }

        public string FileContent { get; set; } = string.Empty;
        public List<VentaCompleta> Ventas { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;

        private ElementReference csvInput;
        private ElementReference jsonInput;
        private ElementReference xmlInput;

        private string TipoArchivo = "";

        // -----------------------------------------------------------
        // LECTURA DEL ARCHIVO
        // -----------------------------------------------------------
        private async Task<string> LeerArchivo(IBrowserFile file)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return await reader.ReadToEndAsync();
        }

        // -----------------------------------------------------------
        // CSV
        // -----------------------------------------------------------
        private async Task OnCsvSelected(InputFileChangeEventArgs e)
        {
            try
            {
                TipoArchivo = "CSV";

                FileContent = await LeerArchivo(e.File);
                ProcesarCSV(FileContent);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al leer CSV: {ex.Message}";
            }
        }

        // -----------------------------------------------------------
        // JSON
        // -----------------------------------------------------------
        private async Task OnJsonSelected(InputFileChangeEventArgs e)
        {
            try
            {
                TipoArchivo = "JSON";

                FileContent = await LeerArchivo(e.File);
                Ventas = DeserializarJson(FileContent);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al leer JSON: {ex.Message}";
            }
        }

        // -----------------------------------------------------------
        // XML
        // -----------------------------------------------------------
        private async Task OnXmlSelected(InputFileChangeEventArgs e)
        {
            try
            {
                TipoArchivo = "XML";

                FileContent = await LeerArchivo(e.File);
                Ventas = ProcesarXML(FileContent);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al leer XML: {ex.Message}";
            }
        }

        // -----------------------------------------------------------
        // PROCESAR CSV
        // -----------------------------------------------------------
        private void ProcesarCSV(string contenido)
        {
            Ventas.Clear();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StringReader(contenido);
            using var csv = new CsvReader(reader, config);

            Ventas = csv.GetRecords<VentaCompleta>().ToList();
        }

        // -----------------------------------------------------------
        // PROCESAR JSON
        // -----------------------------------------------------------
        private List<VentaCompleta> DeserializarJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (json.Trim().StartsWith("["))
                return JsonSerializer.Deserialize<List<VentaCompleta>>(json, options) ?? new();

            var obj = JsonSerializer.Deserialize<VentaCompleta>(json, options);
            return obj != null ? new List<VentaCompleta> { obj } : new();
        }

        // -----------------------------------------------------------
        // PROCESAR XML
        // -----------------------------------------------------------
        private List<VentaCompleta> ProcesarXML(string xmlContent)
        {
            var lista = new List<VentaCompleta>();

            var doc = XDocument.Parse(xmlContent);
            var nodos = doc.Descendants("Venta");
            if (!nodos.Any())
                nodos = doc.Elements("Venta");

            foreach (var nodo in nodos)
            {
                try
                {
                    var venta = new VentaCompleta
                    {
                        VentaId = Convert.ToInt32((string?)nodo.Element("VentaId") ?? "0"),

                        Fecha = DateTime.TryParse((string?)nodo.Element("Fecha"), out var fecha) ? fecha : default,

                        Folio = Convert.ToInt32((string?)nodo.Element("Folio") ?? "0"),

                        ClienteId = Convert.ToInt32((string?)nodo.Element("ClienteId") ?? "0"),

                        NombreCliente = (string?)nodo.Element("NombreCliente") ?? "",
                        Telefono = (string?)nodo.Element("Telefono") ?? "",
                        Domicilio = (string?)nodo.Element("Domicilio") ?? "",

                        ProductoId = Convert.ToInt32((string?)nodo.Element("ProductoId") ?? "0"),
                        SKU = (string?)nodo.Element("SKU") ?? "",
                        DescripcionProducto = (string?)nodo.Element("DescripcionProducto") ?? "",

                        Cantidad = Convert.ToInt32((string?)nodo.Element("Cantidad") ?? "0"),
                        ValorUnitario = Convert.ToDecimal((string?)nodo.Element("ValorUnitario") ?? "0"),
                        Importe = Convert.ToDecimal((string?)nodo.Element("Importe") ?? "0"),
                        TotalVenta = Convert.ToDecimal((string?)nodo.Element("TotalVenta") ?? "0")
                    };

                    lista.Add(venta);
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error procesando nodo XML: {ex.Message}";
                }
            }

            return lista;
        }

        // -----------------------------------------------------------
        // GUARDAR EN MONGO SEGÚN ORIGEN
        // -----------------------------------------------------------
        private async Task GuardarEnMongo()
        {
            try
            {
                if (Ventas.Count > 0)
                    await MongoService.InsertManyAsync(TipoArchivo, Ventas);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al guardar en MongoDB: {ex.Message}";
            }
        }
    }
}

