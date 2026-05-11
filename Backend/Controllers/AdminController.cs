using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly DataContext _context;

    public AdminController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AdminLoginDto loginData)
    {
        if (loginData == null || string.IsNullOrEmpty(loginData.Mail) || string.IsNullOrEmpty(loginData.Geslo))
        {
            return BadRequest(new { message = "Podatki niso pravilno poslani." });
        }

        // Admin ni več zapisan v kodi; preveri se kot uporabnik z vlogo Admin v bazi.
        var adminVrsta = await _context.VrsteUporabnikov
            .FirstOrDefaultAsync(v => v.ime.ToLower() == "admin");

        if (adminVrsta == null)
        {
            return Unauthorized(new { message = "Admin vloga ne obstaja." });
        }

        var admin = await _context.Uporabniki.FirstOrDefaultAsync(u =>
            u.mail.ToLower() == loginData.Mail.ToLower()
            && u.geslo == loginData.Geslo
            && u.vrste_uporabnikov_id == adminVrsta.id);

        if (admin == null)
        {
            return Unauthorized(new { message = "Napačna e-pošta ali geslo." });
        }

        return Ok(new
        {
            id = admin.id,
            ime = admin.ime,
            priimek = admin.priimek,
            mail = admin.mail,
            role = "admin"
        });
    }
}

public class AdminLoginDto
{
    public string Mail { get; set; } = string.Empty;
    public string Geslo { get; set; } = string.Empty;
}
