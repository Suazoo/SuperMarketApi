using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDTO> Register(RegisterDTO dto);
    Task<TokenResponseDTO> RegisterWithRole(RegisterAdminDTO dto);
    Task<TokenResponseDTO?> Login(LoginDTO dto);
    Task<TokenResponseDTO?> RefreshToken(RefreshTokenDTO dto);
}