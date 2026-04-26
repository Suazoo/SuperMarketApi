using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAPI.DTOs;
using SuperMarketAPI.Interfaces;

namespace SuperMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductoController : ControllerBase
{
    private readonly IProductoService _service;

    public ProductoController(IProductoService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]// Permite el acceso a este endpoint sin autenticación
    public async Task<IActionResult> GetAll()
    {
        var productos = await _service.GetAll();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var producto = await _service.GetById(id);
        if (producto == null) return NotFound(new { message = "Producto no encontrado" });
        return Ok(producto);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CrearProductoDTO dto)
    {
        var producto = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = producto.Id }, producto);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]// Solo los usuarios con el rol "Admin" pueden acceder a este endpoint
    public async Task<IActionResult> Update(int id, CrearProductoDTO dto)
    {
        var producto = await _service.Update(id, dto);
        if (producto == null) return NotFound(new { message = "Producto no encontrado" });
        return Ok(producto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        if (!result) return NotFound(new { message = "Producto no encontrado" });
        return Ok(new { message = "Producto eliminado exitosamente" });
    }

    // Nuevo endpoint para obtener productos paginados
    [HttpGet("paged")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaged([FromQuery] PaginacionDTO paginacion)
    {
        var productos = await _service.GetPaged(paginacion);
        return Ok(productos);
    }
}