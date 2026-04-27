using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using SuperMarketAPI.Data;
using SuperMarketAPI.Helpers;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Middleware;
using SuperMarketAPI.Repositories;
using SuperMarketAPI.Services;


// Configurar Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);


// Usar Serilog como logger
builder.Host.UseSerilog();

// Servicios
builder.Services.AddControllers();
// Validación
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddOpenApi();

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "http://localhost:5173",
                "https://www.figma.com",
                "https://figma.com",
                // 👇 Lovable
                "https://id-preview--851fd874-c1a0-4c0c-ba60-24e2ae0ec2e7.lovable.app",
                "https://851fd874-c1a0-4c0c-ba60-24e2ae0ec2e7.lovableproject.com",
                "https://project--851fd874-c1a0-4c0c-ba60-24e2ae0ec2e7.lovable.app",
                "https://project--851fd874-c1a0-4c0c-ba60-24e2ae0ec2e7-dev.lovable.app"
              )
              .SetIsOriginAllowedToAllowWildcardSubdomains() // por si Lovable cambia subdominios
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.RejectionStatusCode = 429;
});

// Inyección de dependencias
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtHelper>();

// JWT Authentication
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

var app = builder.Build();

// ===== PIPELINE DE MIDDLEWARE (el orden importa) =====

// 1. Excepciones - envuelve todo para capturar errores
app.UseMiddleware<ExceptionMiddleware>();

// 2. Response Time - mide el tiempo de cada request
app.UseMiddleware<ResponseTimeMiddleware>();

// 3. Logging - registra entrada y salida
app.UseMiddleware<LoggingMiddleware>();

// 4. Rate Limiting - protege contra abuso
app.UseRateLimiter();

// 5. CORS - permite peticiones desde el frontend
app.UseCors("AllowFrontend");

// 6. Scalar - documentación (solo en desarrollo)
app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection();

// 7. Auth - autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();
// 8. Controllers - maneja las rutas de la API
app.MapControllers();
app.Run();