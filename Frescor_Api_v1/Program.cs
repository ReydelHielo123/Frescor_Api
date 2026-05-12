using Frescor_Api_v1.Data;
using Frescor_Api_v1.Models;
using Frescor_Api_v1.Resolvers;
using Frescor_Api_v1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PdfSharp.Fonts;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var assembly = Assembly.GetExecutingAssembly();
var services = assembly.GetTypes()
	.Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract);

foreach (var service in services)
{
	var interfaceType = service.GetInterfaces().FirstOrDefault();
	if (interfaceType != null)
	{
		builder.Services.AddScoped(interfaceType, service);
	}
}

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy
			.WithOrigins(
				"http://localhost:4200",
				"https://localhost:4200",
				"https://frescor-app-ulz4.vercel.app",
				"https://frescor-app.vercel.app"
			)
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
		};
	});

builder.Services.AddAuthorization();
builder.Services.AddScoped<JWTService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
	try
	{
		await next();
	}
	catch (Exception ex)
	{
		context.Response.StatusCode = 500;
		await context.Response.WriteAsJsonAsync(new ApiResponse<object>
		{
			Success = false,
			Message = ex.Message, // cambiá esto temporalmente
			Data = null
		});
	}
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();