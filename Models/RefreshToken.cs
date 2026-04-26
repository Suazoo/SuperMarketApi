namespace SuperMarketAPI.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}