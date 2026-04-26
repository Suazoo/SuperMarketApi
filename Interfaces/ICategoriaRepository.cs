using SuperMarketAPI.DTOs;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Interfaces;

public interface ICategoriaRepository
{
    Task<IEnumerable<Categoria>> GetAll();
    Task<(IEnumerable<Categoria> Items, int TotalCount)> GetPaged(int pageNumber, int pageSize);
    Task<Categoria?> GetById(int id);
    Task<Categoria> Create(Categoria categoria);
    Task<Categoria?> Update(int id, Categoria categoria);
    Task<bool> Delete(int id);
}