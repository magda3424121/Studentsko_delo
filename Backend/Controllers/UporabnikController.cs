using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UporabnikController : ControllerBase
{
    private readonly DataContext _context;

    public UporabnikController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Uporabnik>>> GetUporabniki()
    {
        return await _context.Uporabniki.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Uporabnik>> GetUporabnik(int id)
    {
        var uporabnik = await _context.Uporabniki.FindAsync(id);
        if (uporabnik == null) return NotFound();

        return Ok(uporabnik);
    }

    [HttpPost]
    public async Task<ActionResult<Uporabnik>> PostUporabnik(Uporabnik uporabnik)
    {
        _context.Uporabniki.Add(uporabnik);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUporabnik), new { id = uporabnik.id }, uporabnik);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutUporabnik(int id, Uporabnik uporabnik)
    {
        if (id != uporabnik.id) return BadRequest();

        _context.Entry(uporabnik).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUporabnik(int id)
    {
        var uporabnik = await _context.Uporabniki.FindAsync(id);
        if (uporabnik == null) return NotFound();

        _context.Uporabniki.Remove(uporabnik);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
