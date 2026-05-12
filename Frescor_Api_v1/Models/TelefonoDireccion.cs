using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models
{
	[Table("Telefono_Direccion")]
	public class TelefonoDireccion
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Telefono { get; set; } = null!;
		public string Direccion { get; set; } = null!;
		[Column("Direccion_Mayusculas")]
		public string? DireccionMayusculas { get; set; }
		public int? Zona { get; set; }
		public int CuponDescuento { get; set; }
		public string? NombreCupon { get; set; }
		public string? OtrosMediosPago { get; set; }
	}
}