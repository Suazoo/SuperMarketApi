using Microsoft.EntityFrameworkCore;
using SuperMarketAPI.Data;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Usuario> Create(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public Task<Usuario?> GetByEmail(string email)
      => _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
}