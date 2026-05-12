using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models
{
	[Table("Pedido")]
	public class Order
	{
		public int Id { get; set; }
		public string Direccion { get; set; }
		[Column("10,5kg")]
		public int Kg10_5 { get; set; }

		[Column("2,5kg")]
		public int Kg2_5 { get; set; }

		[Column("1,5kg")]
		public int Kg1_5 { get; set; }
		public int Triturado { get; set; }
		public DateTime Fecha_Carga { get; set; }
		public string Precio_Total { get; set; }
		public int Zona { get; set; }
		public string FormaPago { get; set; }
	}
}
