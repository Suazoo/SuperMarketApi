using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Interfaces;

public interface ICategoriaService
{
    Task<IEnumerable<CategoriaResponseDTO>> GetAll();
    Task<CategoriaResponseDTO?> GetById(int id);
    Task<CategoriaResponseDTO> Create(CrearCategoriaDTO dto);
    Task<CategoriaResponseDTO?> Update(int id, CrearCategoriaDTO dto);
    Task<bool> Delete(int id);
}