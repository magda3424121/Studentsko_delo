using Microsoft.AspNetCore.Mvc;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;

    public AuthController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Uporabnik uporabnik)
    {
        try 
        {
            // Nastavimo privzete vrednosti, če niso poslane
            if (uporabnik.vrste_uporabnikov_id == 0) uporabnik.vrste_uporabnikov_id = 1;
            if (uporabnik.kraji_id == 0) uporabnik.kraji_id = 1;

            _context.Uporabniki.Add(uporabnik);
            
            // SHRANJEVANJE V BAZO (prej je bilo to v isti vrstici kot komentar!)
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "Registracija uspešna!" });
        }
        catch (Exception ex)
        {
            // To nam bo v terminalu izpisalo, zakaj točno baza zavrača podatke
            Console.WriteLine("NAPAKA PRI REGISTRACIJI: " + ex.Message);
            return BadRequest(new { message = "Napaka na strežniku: " + ex.Message });
        }
    }
    [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginModel model)
{
    // Poiščemo uporabnika po mailu in geslu (v pravi aplikaciji bi gesla šifrirali!)
    var uporabnik = _context.Uporabniki
        .FirstOrDefault(u => u.mail == model.Mail && u.geslo == model.Geslo);

    if (uporabnik == null)
    {
        return Unauthorized(new { message = "Napačen e-naslov ali geslo!" });
    }

    // Vrnemo podatke o uporabniku (brez gesla), da jih JS shrani
    return Ok(new { 
        ime = uporabnik.ime, 
        priimek = uporabnik.priimek,
        mail = uporabnik.mail
    });
}

// Pomožni razred za prejem podatkov
public class LoginModel {
    public string Mail { get; set; } = string.Empty;
    public string Geslo { get; set; } = string.Empty;
}

}