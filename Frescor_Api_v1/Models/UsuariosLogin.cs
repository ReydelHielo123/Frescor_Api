using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Frescor_Api_v1.Models
{
	[Table("Usuarios_Login")]
	public class UsuariosLogin
	{
		[Key]
		public int UserID { get; set; }
		public string? Usuario { get; set; }
		public string? Contraseña { get; set; }
		public string? Rol { get; set; }
	}
}