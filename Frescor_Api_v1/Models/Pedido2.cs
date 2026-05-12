using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models;

[Table("Pedido_2")]
public partial class Pedido2
{
	public int Id { get; set; }
	public string? Direccion { get; set; }
	[Column("kg10_5")]
	public int? Kg10_5 { get; set; }
	[Column("kg2_5")]
	public int? Kg2_5 { get; set; }
	[Column("kg1_5")]
	public int? Kg1_5 { get; set; }
	public int? Triturado { get; set; }
	[Column("Fecha_Carga")]
	public DateTime? Fecha_Carga { get; set; }
	[Column("Precio_Total")]
	public string? Precio_Total { get; set; }
	public int? Zona { get; set; }
	[Column("FormaPago")]
	public string? FormaPago { get; set; }
	public string? PaymentId { get; set; }
	public string? Estado { get; set; }
	[Column("kg4_5")]
	public int? Kg4_5 { get; set; }
}