using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Frescor_Api_v1.Models
{
    [Table("BoletaItem")]
    public class BoletaItem
    {
        public int Id { get; set; }
        public int BoletaId { get; set; }
        public string TipoBolsa { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public Boleta? Boleta { get; set; }
    }
}
