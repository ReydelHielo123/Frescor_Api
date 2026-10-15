namespace Frescor_Api_v1.Models.Responses;

// Datos visibles para el cliente, sin dirección ni identificadores de pago.
public class PedidoRecienteResponse
{
    public int Id { get; set; }
    public int Kg1_5 { get; set; }
    public int Kg2_5 { get; set; }
    public int Kg4_5 { get; set; }
    public int Kg10_5 { get; set; }
    public int Triturado { get; set; }
    public DateTime? Fecha_Carga { get; set; }
    public string Precio_Total { get; set; } = "0";
    public string FormaPago { get; set; } = "";
    public string Estado { get; set; } = "";
}

public class PedidosRecientesResponse : PaginatedResponse<PedidoRecienteResponse>
{
    public DateTime Desde { get; set; }
    public DateTime Hasta { get; set; }
}
