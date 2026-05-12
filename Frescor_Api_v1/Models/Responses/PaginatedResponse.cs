namespace Frescor_Api_v1.Models.Responses
{
	public class PaginatedResponse<T>
	{
		public List<T> Items { get; set; } = new();
		public int TotalRegistros { get; set; }
		public int Pagina { get; set; }
		public int TamañoPagina { get; set; }
		public int TotalPaginas => (int)Math.Ceiling((double)TotalRegistros / TamañoPagina);
	}
}