using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KrajController : ControllerBase
{
    private readonly DataContext _context;

    public KrajController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Kraj>>> GetKraji()
    {
        return await _context.Kraji.OrderBy(k => k.ime).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Kraj>> GetKraj(int id)
    {
        var kraj = await _context.Kraji.FindAsync(id);
        if (kraj == null) return NotFound();

        return Ok(kraj);
    }

    [HttpPost]
    public async Task<ActionResult<Kraj>> PostKraj(Kraj kraj)
    {
        _context.Kraji.Add(kraj);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetKraj), new { id = kraj.id }, kraj);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutKraj(int id, Kraj kraj)
    {
        if (id != kraj.id) return BadRequest();

        _context.Entry(kraj).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteKraj(int id)
    {
        var kraj = await _context.Kraji.FindAsync(id);
        if (kraj == null) return NotFound();

        _context.Kraji.Remove(kraj);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
