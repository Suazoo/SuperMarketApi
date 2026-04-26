using SuperMarketAPI.DTOs;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Services;

public class ProductoService : IProductoService
{
    private readonly IProductoRepository _repository;

    public ProductoService(IProductoRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProductoResponseDTO> Create(CrearProductoDTO dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            CategoriaId = dto.CategoriaId
        };

        var result = await _repository.Create(producto);
        return new ProductoResponseDTO
        {
            Id = result!.Id,
            Nombre = result.Nombre,
            Descripcion = result.Descripcion,
            Precio = result.Precio,
            Stock = result.Stock,
            Categoria = result.Categoria.Nombre,
            FechaCreacion = result.FechaCreacion
        };
    }

    public async Task<bool> Delete(int id)
        => await _repository.Delete(id);

    public async Task<IEnumerable<ProductoResponseDTO>> GetAll()
    {
        var productos = await _repository.GetAll();
        return productos.Select(p => new ProductoResponseDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Stock = p.Stock,
            Categoria = p.Categoria.Nombre,
            FechaCreacion = p.FechaCreacion
        });
    }

    public async Task<ProductoResponseDTO?> GetById(int id)
    {
        var producto = await _repository.GetById(id);
        if (producto ==null) return null;

        return new ProductoResponseDTO
        {
            Id = producto.Id,
            Nombre = producto.Nombre,
            Descripcion = producto.Descripcion,
            Precio = producto.Precio,
            Stock = producto.Stock,
            Categoria = producto.Categoria.Nombre,
            FechaCreacion = producto.FechaCreacion
        };
    }

    public async Task<ProductoResponseDTO?> Update(int id, CrearProductoDTO dto)
    {
        var producto = new Producto
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Stock = dto.Stock,
            CategoriaId = dto.CategoriaId
        };

        var update = await _repository.Update(id, producto);
        if (update == null) return null;

        var result = await _repository.GetById(update.Id);
        return new ProductoResponseDTO
        {
            Id = result!.Id,
            Nombre = result.Nombre,
            Descripcion = result.Descripcion,
            Precio = result.Precio,
            Stock = result.Stock,
            Categoria = result.Categoria.Nombre,
            FechaCreacion = result.FechaCreacion
        };
    }

    // Implementación del método para obtener productos paginados
    public async Task<PagedResponseDTO<ProductoResponseDTO>> GetPaged(PaginacionDTO paginacion)
    {
        var (items, totalCount) = await _repository.GetPaged(paginacion.PageNumber, paginacion.PageSize);

        return new PagedResponseDTO<ProductoResponseDTO>
        {
            Data = items.Select(p => new ProductoResponseDTO
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                Categoria = p.Categoria.Nombre,
                FechaCreacion = p.FechaCreacion
            }),
            PageNumber = paginacion.PageNumber,
            PageSize = paginacion.PageSize,
            TotalRecords = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)paginacion.PageSize)
        };
    }
}