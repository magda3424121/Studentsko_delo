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

    // Pridobi vse objave - zdaj bo vključil tudi stolpec lokacija
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Objava>>> GetJobs()
    {
        return await _context.Objave.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Objava>> GetJob(int id)
    {
        var job = await _context.Objave.FindAsync(id);
        if (job == null) return NotFound();
        return Ok(job);
    }

    [HttpPost]
    public async Task<ActionResult<Objava>> PostJob(Objava objava)
    {
        _context.Objave.Add(objava);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetJob), new { id = objava.id }, objava);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutJob(int id, Objava objava)
    {
        if (id != objava.id) return BadRequest();
        _context.Entry(objava).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJob(int id)
    {
        var job = await _context.Objave.FindAsync(id);
        if (job == null) return NotFound();
        _context.Objave.Remove(job);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}