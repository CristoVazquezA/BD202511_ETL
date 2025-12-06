using CargaArchivos.Entities;
using CargaArchivos.Services;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using System.Text;

namespace CargaArchivos.Components
{
    public partial class CargarArchivo
    {
        [Inject]
        public MongoVentaService MongoService { get; set; }
        public string FileContent { get; set; } = string.Empty;
        public List<VentaCompleta> Ventas { get; set; } = new();

        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file == null)
                return;

            using var stream = file.OpenReadStream(5 * 1024 * 1024);
            using var reader = new StreamReader(stream, Encoding.UTF8);

            FileContent = await reader.ReadToEndAsync();

            ProcesarCSV(FileContent);

            await GuardarEnMongo();    // 🚀 Guardar automáticamente
        }

        private void ProcesarCSV(string contenido)
        {
            Ventas.Clear();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                TrimOptions = TrimOptions.Trim,
                BadDataFound = null
            };

            using var reader = new StringReader(contenido);
            using var csv = new CsvReader(reader, config);

            Ventas = csv.GetRecords<VentaCompleta>().ToList();
        }

        private async Task GuardarEnMongo()
        {
            if (Ventas.Count == 0)
                return;

            await MongoService.InsertManyAsync(Ventas);
        }
    }
}
