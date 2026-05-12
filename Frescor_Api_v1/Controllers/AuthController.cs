using Frescor_Api_v1.Data;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Models.Request;
using Frescor_Api_v1.Models.Responses;
using Frescor_Api_v1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly JWTService _jwtService;
		private readonly ILogger<AuthController> _logger;

		public AuthController(AppDbContext context, JWTService jwtService, ILogger<AuthController> logger)
		{
			_context = context;
			_jwtService = jwtService;
			_logger = logger;
		}

		[HttpPost("login")]
		public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
		{
			_logger.LogInformation("Intento de login para usuario: {Usuario}", request.Usuario);

			var passwordHash = HashPassword(request.Contrasena);

			var usuario = await _context.UsuariosLogin
				.FirstOrDefaultAsync(u => u.Usuario == request.Usuario && u.Contraseña == passwordHash);

			if (usuario == null)
			{
				return Unauthorized(new ApiResponse<LoginResponse>
				{
					Success = false,
					Message = "Usuario o contraseña incorrectos."
				});
			}

			var rol = usuario.Rol ?? "empleado";
			var token = _jwtService.GenerarToken(usuario.Usuario, rol);

			return Ok(new ApiResponse<LoginResponse>
			{
				Success = true,
				Message = "Login exitoso",
				Data = new LoginResponse
				{
					Token = token,
					Usuario = usuario.Usuario,
					Rol = rol
				}
			});
		}


		#region PRIVATEMETHODS
		private string HashPassword(string password)
		{
			using var sha256 = System.Security.Cryptography.SHA256.Create();
			var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return BitConverter.ToString(bytes).Replace("-", "").ToUpper();
		}

		#endregion
	}
}
