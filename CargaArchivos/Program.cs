using CargaArchivos.Commands;
using CargaArchivos.Components;
using CargaArchivos.DataBase;

namespace CargaArchivos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔹 Leer cadena de conexión
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 🔹 Registrar tu clase SQLServer e inyectar la connection string
            builder.Services.AddScoped<SQLServer>(sp =>
            {
                return new SQLServer(connectionString!);
            });

            builder.Services.AddScoped<ClienteCommands>();
            builder.Services.AddScoped<ProductoCommands>();
            builder.Services.AddScoped<VentaCommands>();
            builder.Services.AddScoped<VentaDetalleCommands>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
