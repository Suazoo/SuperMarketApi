using SuperMarketAPI.Models;

namespace SuperMarketAPI.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmail(string email);
    Task<Usuario> Create(Usuario usuario);
}