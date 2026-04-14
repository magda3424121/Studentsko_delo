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

    [HttpPost]
    public async Task<ActionResult<Uporabnik>> PostUporabnik(Uporabnik uporabnik)
    {
        _context.Uporabniki.Add(uporabnik);
        await _context.SaveChangesAsync();
        return Ok(uporabnik);
    }
}