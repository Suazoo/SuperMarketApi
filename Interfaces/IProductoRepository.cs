using SuperMarketAPI.DTOs;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Interfaces;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> GetAll();
    Task<(IEnumerable<Producto> Items, int TotalCount)> GetPaged(int pageNumber, int pageSize);
    Task<Producto?> GetById(int id);
    Task<Producto> Create(Producto producto);
    Task<Producto?> Update(int id, Producto producto);
    Task<bool> Delete(int id);
}