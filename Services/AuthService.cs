using SuperMarketAPI.DTOs;
using SuperMarketAPI.Helpers;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(IUsuarioRepository repository, JwtHelper jwtHelper)
    {
        _repository = repository;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDTO?> Login(LoginDTO dto)
    {
        var usuario = await _repository.GetByEmail(dto.Email);
        if (usuario == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash))
            return null;

        var token = _jwtHelper.GenerarToken(usuario);
        return new AuthResponseDTO
        {
            Token = token,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }
    public async Task<AuthResponseDTO> Register(RegisterDTO dto)
    {
        var esistingUser = await _repository.GetByEmail(dto.Email);
        if (esistingUser != null)
            throw new Exception("Ya existe un usuario con este correo electrónico.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        var created = await _repository.Create(usuario);
        var token = _jwtHelper.GenerarToken(created);

        return new AuthResponseDTO
        {
            Token = token,
            Nombre = created.Nombre,
            Email = created.Email,
            Rol = created.Rol
        };
    }  
}