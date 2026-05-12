using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models;

[Table("Zonas")]
public partial class Zona
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Column("Zona")]
	public int? Zona1 { get; set; }
	public string? Barrio { get; set; }
}