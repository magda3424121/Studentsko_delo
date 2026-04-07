using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PodjetjeController : ControllerBase
{
    private readonly DataContext _context;

    public PodjetjeController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Podjetje>>> GetPodjetja()
    {
        return await _context.Podjetja.ToListAsync();
    }
}