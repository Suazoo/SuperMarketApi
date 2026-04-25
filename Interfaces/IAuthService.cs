using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> Register(RegisterDTO dto);
    Task<AuthResponseDTO?> Login(LoginDTO dto);
}