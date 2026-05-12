using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models;

[Table("Cupones")]
public partial class Cupone
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string Codigo { get; set; } = null!;
	public decimal Porcentaje { get; set; }
	public string? Descripcion { get; set; }
}