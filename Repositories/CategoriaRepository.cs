using Microsoft.EntityFrameworkCore;
using SuperMarketAPI.Data;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Repositories;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly AppDbContext _context;
    public CategoriaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Categoria> Create(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return categoria;
    }

    public async Task<bool> Delete(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null) return false;

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Categoria>> GetAll()
    {
        return await _context.Categorias.ToListAsync();
    }
    
    
    public async Task<Categoria?> GetById(int id)
        => await _context.Categorias.FindAsync(id);

    public async Task<Categoria?> Update(int id, Categoria categoria)
    {
        var existing = await _context.Categorias.FindAsync(id);
        if (existing == null) return null;

        existing.Nombre = categoria.Nombre;
        existing.Descripcion = categoria.Descripcion;
        await _context.SaveChangesAsync();
        return existing;
    }
}