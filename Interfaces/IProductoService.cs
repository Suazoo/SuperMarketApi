using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<ProductoResponseDTO>> GetAll();
    Task<ProductoResponseDTO?> GetById(int id);
    Task<ProductoResponseDTO> Create(CrearProductoDTO dto);
    Task<ProductoResponseDTO?> Update(int id, CrearProductoDTO dto);
    Task<bool> Delete(int id);
}