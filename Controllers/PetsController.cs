using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using patinhasemdia.Data;
using patinhasemdia.Models;

namespace patinhasemdia.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly AppDbContext _context;
    public PetsController(AppDbContext context) => _context = context;

    /// <summary>
    /// Lista todos os pets cadastrados
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pets = await _context.Pets.Include(p => p.Tutor).ToListAsync();
        return Ok(pets);
    }

    /// <summary>
    /// Busca um pet pelo ID
    /// </summary>
    /// <param name="id">ID do pet</param>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var pet = await _context.Pets.Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id);
        return pet is null ? NotFound() : Ok(pet);
    }

    /// <summary>
    /// Busca todos os pets de um tutor
    /// </summary>
    /// <param name="tutorId">ID do tutor</param>
    [HttpGet("tutor/{tutorId:int}")]
    public async Task<IActionResult> GetByTutor(int tutorId)
    {
        var pets = await _context.Pets.Where(p => p.TutorId == tutorId).ToListAsync();
        return Ok(pets);
    }

    /// <summary>
    /// Retorna resumo de saúde do pet com contagem de eventos
    /// </summary>
    /// <param name="id">ID do pet</param>
    [HttpGet("{id:int}/resumo-saude")]
    public async Task<IActionResult> ResumoSaude(int id)
    {
        var pet = await _context.Pets.Include(p => p.Eventos).FirstOrDefaultAsync(p => p.Id == id);
        if (pet is null) return NotFound();

        var resumo = new
        {
            pet.Nome,
            TotalEventos = pet.Eventos.Count,
            Pendentes = pet.Eventos.Count(e => e.Status == "PENDENTE"),
            Concluidos = pet.Eventos.Count(e => e.Status == "CONCLUIDO"),
            Atrasados = pet.Eventos.Count(e => e.Status == "ATRASADO")
        };
        return Ok(resumo);
    }

    /// <summary>
    /// Cria um novo pet
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(Pet pet)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = pet.Id }, pet);
    }

    /// <summary>
    /// Atualiza os dados de um pet existente
    /// </summary>
    /// <param name="id">ID do pet</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Pet pet)
    {
        if (id != pet.Id) return BadRequest();
        _context.Entry(pet).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Remove um pet pelo ID
    /// </summary>
    /// <param name="id">ID do pet</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet is null) return NotFound();
        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}