using SuperMarketAPI.Models;

namespace SuperMarketAPI.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAll();
    Task<Producto?> GetById(int id);
    Task<Producto> Create(Producto producto);
    Task<Producto?> Update(int id, Producto producto);
    Task<bool> Delete(int id);
}