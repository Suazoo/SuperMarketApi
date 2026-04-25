using SuperMarketAPI.DTOs;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Services;

public class CategoriaService : ICategoriaService
{

    private readonly ICategoriaRepository _repository;

    public CategoriaService(ICategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<CategoriaResponseDTO> Create(CrearCategoriaDTO dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        var created = await _repository.Create(categoria);
        return new CategoriaResponseDTO
        {
            Id = created.Id,
            Nombre = created.Nombre,
            Descripcion = created.Descripcion,
            FechaCreacion = created.FechaCreacion
        };
    }

    public async Task<bool> Delete(int id)
      => await _repository.Delete(id);

    public async Task<IEnumerable<CategoriaResponseDTO>> GetAll()
    {
        var categorias = await _repository.GetAll();
        return categorias.Select(c => new CategoriaResponseDTO
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Descripcion = c.Descripcion,
            FechaCreacion = c.FechaCreacion
        });
    }

    public async Task<CategoriaResponseDTO?> GetById(int id)
    {
        var categoria = await _repository.GetById(id);
        if (categoria == null) return null;

        return new CategoriaResponseDTO
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            FechaCreacion = categoria.FechaCreacion
        };
    }

    public async Task<CategoriaResponseDTO?> Update(int id, CrearCategoriaDTO dto)
    {
        var categoria = new Categoria
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion
        };

        var update = await _repository.Update(id, categoria);
        if (update == null) return null;

        return new CategoriaResponseDTO
        {
            Id = update.Id,
            Nombre = update.Nombre,
            Descripcion = update.Descripcion,
            FechaCreacion = update.FechaCreacion
        };
    }
}