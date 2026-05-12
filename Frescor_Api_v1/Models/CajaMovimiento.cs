using System;
using System.Collections.Generic;

namespace Frescor_Api_v1.Models;

public partial class CajaMovimiento
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Monto { get; set; }

    public string? Concepto { get; set; }

    public string? MedioPago { get; set; }

    public int? PedidoId { get; set; }

    public string? Usuario { get; set; }

    public string? Observaciones { get; set; }
}
