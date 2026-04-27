namespace SuperMarketAPI.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    
    public bool IsRevoked { get; set; } = false;
    public DateTime Expiration { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}