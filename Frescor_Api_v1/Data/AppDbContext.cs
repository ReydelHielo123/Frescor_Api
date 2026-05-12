using Frescor_Api_v1.Models;
using Microsoft.EntityFrameworkCore;

namespace Frescor_Api_v1.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public DbSet<Order> Orders { get; set; }
		public DbSet<TelefonoDireccion> Telefonos { get; set; }
		public DbSet<Precio> Precios { get; set; }
		public DbSet<Pedido2> Pedido2 { get; set; }
		public DbSet<UsuariosLogin> UsuariosLogin { get; set; }
		public DbSet<Cupone> Cupones { get; set; }
		public DbSet<Zona> Zonas { get; set; }
	}
}