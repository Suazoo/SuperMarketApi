using SuperMarketAPI.Models;

namespace SuperMarketAPI.Models;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Relación: un producto pertenece a una categoría
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
}