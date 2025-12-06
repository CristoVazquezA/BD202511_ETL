namespace CargaArchivos.Entities
{
    public class VentaCompleta
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }
        public int Folio { get; set; }
        public int ClienteId { get; set; }

        public string? NombreCliente { get; set; }
        public string? Telefono { get; set; }
        public string? Domicilio { get; set; }
        public int ProductoId { get; set; }
        public string? SKU { get; set; }
        public string? DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Importe { get; set; }
        public decimal TotalVenta { get; set; }
    }
}
