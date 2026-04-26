using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using SuperMarketAPI.Data;
using SuperMarketAPI.DTOs;
using SuperMarketAPI.Helpers;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly AppDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IUsuarioRepository repository, AppDbContext context, JwtHelper jwtHelper)
    {
        _repository = repository;
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<TokenResponseDTO> Register(RegisterDTO dto)
    {
        var existingUser = await _repository.GetByEmail(dto.Email);
        if (existingUser != null)
            throw new Exception("Ya existe un usuario con ese email");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        var created = await _repository.Create(usuario);
        return await GenerateTokenResponse(created);
    }

    public async Task<TokenResponseDTO> RegisterWithRole(RegisterAdminDTO dto)
    {
        var existingUser = await _repository.GetByEmail(dto.Email);
        if (existingUser != null)
            throw new Exception("Ya existe un usuario con ese email");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        var created = await _repository.Create(usuario);
        return await GenerateTokenResponse(created);
    }

    public async Task<TokenResponseDTO?> Login(LoginDTO dto)
    {
        var usuario = await _repository.GetByEmail(dto.Email);
        if (usuario == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        return await GenerateTokenResponse(usuario);
    }

    public async Task<TokenResponseDTO?> RefreshToken(RefreshTokenDTO dto)
    {
        var storedToken = await _context.RefreshTokens
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Token == dto.RefreshToken && !r.IsRevoked);

        if (storedToken == null || storedToken.Expiration < DateTime.Now)
            return null;

        // Revocar el refresh token usado
        storedToken.IsRevoked = true;
        await _context.SaveChangesAsync();

        return await GenerateTokenResponse(storedToken.Usuario);
    }

    private async Task<TokenResponseDTO> GenerateTokenResponse(Usuario usuario)
    {
        var jwt = _jwtHelper.GenerarToken(usuario);
        var refreshToken = GenerateRefreshToken();

        var token = new RefreshToken
        {
            Token = refreshToken,
            Expiration = DateTime.Now.AddDays(7),
            UsuarioId = usuario.Id
        };

        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();

        return new TokenResponseDTO
        {
            Token = jwt,
            RefreshToken = refreshToken,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}