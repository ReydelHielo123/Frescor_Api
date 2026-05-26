using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models
{
    [Table("Boleta")]
    public class Boleta
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public string Telefono { get; set; } = null!;
        public string? Direccion { get; set; }
        public DateTime FechaBoleta { get; set; }
        public decimal ImporteTotal { get; set; }
        public decimal? ImporteConDescuento { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public DateTime FechaCarga { get; set; } = DateTime.UtcNow;
        public List<BoletaItem> Items { get; set; } = new();
    }
}
