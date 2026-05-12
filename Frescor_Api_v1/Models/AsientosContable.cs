using System;
using System.Collections.Generic;

namespace Frescor_Api_v1.Models;

public partial class AsientosContable
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public string CuentaContable { get; set; } = null!;

    public decimal? Debe { get; set; }

    public decimal? Haber { get; set; }

    public string? Descripcion { get; set; }

    public int? PedidoId { get; set; }

    public string? Usuario { get; set; }
}
