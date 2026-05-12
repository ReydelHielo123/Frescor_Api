using System;
using System.Collections.Generic;

namespace Frescor_Api_v1.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public string? Direccion { get; set; }

    public int? _105kg { get; set; }

    public int? _25kg { get; set; }

    public int? _15kg { get; set; }

    public int? Triturado { get; set; }

    public DateTime? FechaCarga { get; set; }

    public string? PrecioTotal { get; set; }

    public int? Zona { get; set; }

    public string? FormaPago { get; set; }

    public int? _45kg { get; set; }
}
