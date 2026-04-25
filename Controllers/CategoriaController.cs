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
    public async Task<IActionResult> GetAll()
    {
        var categoria = await _service.GetAll();
        return Ok(categoria);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var categoria = await _service.GetById(id);
        if (categoria == null) return NotFound(new {message = "categoria no encontrada"});
        return Ok(categoria);
    }


    [HttpPost]
    public async Task<IActionResult> Create(CrearCategoriaDTO dto)
    {
        var categoria = await _service.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CrearCategoriaDTO dto)
    {
        var categoria = await _service.Update(id, dto);
        if (categoria == null) return NotFound(new {message = "categoria no encontrada"});
        return Ok(categoria);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id);
        if (!result) return NotFound(new {message = "categoria no encontrada"});
        return Ok(new {message = "categoria eliminada exitosamente"});
    }


}