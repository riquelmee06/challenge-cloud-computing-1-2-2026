using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using patinhasemdia.Data;
using patinhasemdia.Models;

namespace patinhasemdia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController : ControllerBase
{
    private readonly AppDbContext _context;
    public EventosController(AppDbContext context) => _context = context;

    /// <summary>
    /// Lista todos os eventos de cuidado
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Eventos.Include(e => e.Pet).ToListAsync());

    /// <summary>
    /// Busca todos os eventos de um pet
    /// </summary>
    /// <param name="petId">ID do pet</param>
    [HttpGet("pet/{petId:int}")]
    public async Task<IActionResult> GetByPet(int petId)
    {
        var eventos = await _context.Eventos.Where(e => e.PetId == petId).ToListAsync();
        return Ok(eventos);
    }

    /// <summary>
    /// Busca eventos pelo status
    /// </summary>
    /// <param name="status">Status do evento (PENDENTE, CONCLUIDO, ATRASADO)</param>
    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var eventos = await _context.Eventos
            .Where(e => e.Status.ToUpper() == status.ToUpper())
            .Include(e => e.Pet)
            .ToListAsync();
        return Ok(eventos);
    }

    /// <summary>
    /// Lista todos os eventos com data passada e status pendente
    /// </summary>
    [HttpGet("atrasados")]
    public async Task<IActionResult> GetAtrasados()
    {
        var atrasados = await _context.Eventos
            .Where(e => e.DataPrevista < DateTime.Now && e.Status == "PENDENTE")
            .Include(e => e.Pet)
            .ToListAsync();
        return Ok(atrasados);
    }

    /// <summary>
    /// Cria um novo evento de cuidado
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(EventoCuidado evento)
    {
        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByPet), new { petId = evento.PetId }, evento);
    }

    /// <summary>
    /// Atualiza um evento de cuidado existente
    /// </summary>
    /// <param name="id">ID do evento</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, EventoCuidado evento)
    {
        if (id != evento.Id) return BadRequest();
        _context.Entry(evento).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove um evento de cuidado pelo ID
    /// </summary>
    /// <param name="id">ID do evento</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento is null) return NotFound();
        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}