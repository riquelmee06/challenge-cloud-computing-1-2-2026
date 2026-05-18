using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using patinhasemdia.Data;
using patinhasemdia.Models;

namespace patinhasemdia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TutoresController : ControllerBase
{
    private readonly AppDbContext _context;
    public TutoresController(AppDbContext context) => _context = context;

    /// <summary>
    /// Lista todos os tutores com seus pets
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Tutores.Include(t => t.Pets).ToListAsync());

    /// <summary>
    /// Busca um tutor pelo ID com seus pets
    /// </summary>
    /// <param name="id">ID do tutor</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var tutor = await _context.Tutores.Include(t => t.Pets).FirstOrDefaultAsync(t => t.Id == id);
        return tutor is null ? NotFound() : Ok(tutor);
    }

    /// <summary>
    /// Cria um novo tutor
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Tutor tutor)
    {
        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, tutor);
    }

    /// <summary>
    /// Atualiza os dados de um tutor existente
    /// </summary>
    /// <param name="id">ID do tutor</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Tutor tutor)
    {
        if (id != tutor.Id) return BadRequest();
        _context.Entry(tutor).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove um tutor pelo ID
    /// </summary>
    /// <param name="id">ID do tutor</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var tutor = await _context.Tutores.FindAsync(id);
        if (tutor is null) return NotFound();
        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}