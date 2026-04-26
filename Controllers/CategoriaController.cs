using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperMarketAPI.DTOs;
using SuperMarketAPI.Interfaces;
using SuperMarketAPI.Models;

namespace SuperMarketAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriaController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]// Permite el acceso a este endpoint sin autenticación
    public async Task<IActionResult> GetAll()
    {
        var categoria = await _service.GetAll();
        return Ok(categoria);
    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetById(id);
        if (categoria == null) return NotFound(new {message = "categoria no encontrada"});
        return Ok(categoria);
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]// Solo los usuarios con el rol "Admin" pueden acceder a este endpoint
    public async Task<IActionResult> Create(CrearCategoriaDTO dto)
    {
        var categoria = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, CrearCategoriaDTO dto)
    {
        var categoria = await _service.Update(id, dto);
        if (categoria == null) return NotFound(new {message = "categoria no encontrada"});
        return Ok(categoria);
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        if (!result) return NotFound(new {message = "categoria no encontrada"});
        return Ok(new {message = "categoria eliminada exitosamente"});
    }

    // Endpoint para obtener categorías con paginación
    [HttpGet("paged")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPaged([FromQuery] PaginacionDTO paginacion)
    {
        var categorias = await _service.GetPaged(paginacion);
        return Ok(categorias);
    }


}