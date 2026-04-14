using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrijavaController : ControllerBase
{
    private readonly DataContext _context;

    public PrijavaController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Prijava>>> GetPrijave()
    {
        return await _context.Prijave.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Prijava>> PostPrijava(Prijava prijava)
    {
        prijava.datum_prijave = DateTime.UtcNow;
        
        _context.Prijave.Add(prijava);
        await _context.SaveChangesAsync();
        
        return Ok(prijava);
    }
}