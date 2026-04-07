using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobOfferController : ControllerBase
{
    private readonly DataContext _context;

    public JobOfferController(DataContext context)
    {
        _context = context;
    }

    // 1. PRIDOBI VSE OGLASE
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Objava>>> GetJobs()
    {
        return await _context.Objave.ToListAsync();
    }

    // 2. PRIDOBI EN DOLOČEN OGLAS (po ID-ju)
    // Uporabno, ko študent klikne na oglas za več podrobnosti
    [HttpGet("{id}")]
    public async Task<ActionResult<Objava>> GetJob(int id)
    {
        var job = await _context.Objave.FindAsync(id);

        if (job == null)
        {
            return NotFound("Oglas s tem ID-jem ne obstaja.");
        }

        return Ok(job);
    }

    // 3. ISKANJE OGLASOV (po ključni besedi v naslovu)
    // Primer: api/JobOffer/search/natakar
    [HttpGet("search/{keyword}")]
    public async Task<ActionResult<IEnumerable<Objava>>> SearchJobs(string keyword)
    {
        var results = await _context.Objave
            .Where(j => j.ime.ToLower().Contains(keyword.ToLower()))
            .ToListAsync();

        return Ok(results);
    }

    // 4. DODAJ NOVO OBJAVO
    [HttpPost]
    public async Task<ActionResult<Objava>> PostJob(Objava objava)
    {
        _context.Objave.Add(objava);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetJob), new { id = objava.id }, objava);
    }

    // 5. POSODOBI OGLAS (npr. sprememba plače)
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJob(int id, Objava objava)
    {
        if (id != objava.id)
        {
            return BadRequest("ID se ne ujema.");
        }

        _context.Entry(objava).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Objave.Any(e => e.id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // 6. IZBRIŠI OGLAS
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var job = await _context.Objave.FindAsync(id);
        if (job == null)
        {
            return NotFound();
        }

        _context.Objave.Remove(job);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}