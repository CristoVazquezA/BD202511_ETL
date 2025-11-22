using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace CargaArchivos.Components
{
    public partial class CargarArchivo
    {
        public string FileContent { get; set; } = string.Empty;

        private async Task LoadFile(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file == null)
                return;

            //if (file.ContentType != "text/plain")
            //{
            //    FileContent = "Solo se permiten archivos .txt";
            //    return;
            //}

            using var stream = file.OpenReadStream(maxAllowedSize: 1024 * 1024); // 1 MB
            using var reader = new StreamReader(stream, Encoding.UTF8);

            FileContent = await reader.ReadToEndAsync();
        }
    }
}
