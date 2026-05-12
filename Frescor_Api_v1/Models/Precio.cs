using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models;

[Table("Precios")]
public class Precio
{
    public int Id { get; set; }
	[Column("TIPO_BOLSA")]

	public string? TipoBolsa { get; set; }
	[Column("CANTIDAD_BOLSA")]

	public string? CantidadBolsa { get; set; }
	[Column("PRECIO")]
	public double? Precio1 { get; set; }
}
