using Microsoft.EntityFrameworkCore;
using SuperMarketAPI.Data;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Repositories;

public class ProductoRepository : IProductoRepository
{
    private readonly AppDbContext _context;

    public ProductoRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task<Producto> Create(Producto producto)
    {
        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        
        // Recargar con la categoría incluida
        await _context.Entry(producto).Reference(p => p.Categoria).LoadAsync();
        return producto;
    }

    public async Task<bool> Delete(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null) return false;

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Producto>> GetAll()
      => await _context.Productos.Include(p => p.Categoria).ToListAsync();

    public async Task<Producto?> GetById(int id)
        => await _context.Productos.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Producto?> Update(int id, Producto producto)
    {
        var existing = await _context.Productos.FindAsync(id);
        if (existing == null) return null;

        existing.Nombre = producto.Nombre;
        existing.Descripcion = producto.Descripcion;
        existing.Precio = producto.Precio;
        existing.Stock = producto.Stock;
        existing.CategoriaId = producto.CategoriaId;
        await _context.SaveChangesAsync();
        return existing;
    }
}