namespace CargaArchivos.Dtos
{
    public class VentaCompleta
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Folio { get; set; }

        // Cliente
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }

        // Producto
        public int ProductoId { get; set; }
        public string SKU { get; set; }
        public string DescripcionProducto { get; set; }

        // Valores de venta
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal TotalVenta { get; set; }
    }
}
