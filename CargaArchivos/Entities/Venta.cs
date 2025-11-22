namespace CargaArchivos.Entities
{
    public class Venta
    {
        public int Id { get; set; }
        public string? Fecha { get; set; }
        public int Folio { get; set; }
        public double Total { get; set; }
        public int ClienteID { get; set; }
        public List<VentaDetalle>? Conceptos { get; set; }
    }
}
