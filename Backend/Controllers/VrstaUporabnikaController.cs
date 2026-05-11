using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VrstaUporabnikaController : ControllerBase
{
    private readonly DataContext _context;

    public VrstaUporabnikaController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VrstaUporabnika>>> GetVrsteUporabnikov()
    {
        return await _context.VrsteUporabnikov.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VrstaUporabnika>> GetVrstaUporabnika(int id)
    {
        var vrsta = await _context.VrsteUporabnikov.FindAsync(id);
        if (vrsta == null) return NotFound();

        return Ok(vrsta);
    }

    [HttpPost]
    public async Task<ActionResult<VrstaUporabnika>> PostVrstaUporabnika(VrstaUporabnika vrsta)
    {
        _context.VrsteUporabnikov.Add(vrsta);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVrstaUporabnika), new { id = vrsta.id }, vrsta);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutVrstaUporabnika(int id, VrstaUporabnika vrsta)
    {
        if (id != vrsta.id) return BadRequest();

        _context.Entry(vrsta).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVrstaUporabnika(int id)
    {
        var vrsta = await _context.VrsteUporabnikov.FindAsync(id);
        if (vrsta == null) return NotFound();

        _context.VrsteUporabnikov.Remove(vrsta);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
