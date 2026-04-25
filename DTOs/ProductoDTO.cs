namespace SuperMarketAPI.DTOs;

public class CrearProductoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public int CategoriaId { get; set; }
}

public class ProductoResponseDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
}