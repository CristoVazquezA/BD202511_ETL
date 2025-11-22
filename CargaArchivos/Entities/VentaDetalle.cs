namespace CargaArchivos.Entities
{
    public class VentaDetalle
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int Renglon { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public double ValorUnitario { get; set; }
        public string? Descripcion { get; set; }
        public double Importe { get; set; }
    }
}
