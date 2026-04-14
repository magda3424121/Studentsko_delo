using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PodjetjeController : ControllerBase
    {
        private readonly DataContext _context;

        public PodjetjeController(DataContext context)
        {
            _context = context;
        }

        // 1. Pridobi vse kraje za dropdown
        [HttpGet("kraji")]
        public async Task<IActionResult> GetKraji()
        {
            var kraji = await _context.Kraji.OrderBy(k => k.ime).ToListAsync();
            return Ok(kraji);
        }

        // 2. Registracija podjetja
        [HttpPost("register")]
        public async Task<ActionResult<Podjetje>> PostPodjetje([FromBody] Podjetje podjetje)
        {
            try 
            {
                _context.Podjetja.Add(podjetje);
                await _context.SaveChangesAsync();
                return Ok(podjetje);
            }
            catch (Exception ex)
            {
                return BadRequest("Napaka pri registraciji: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        // 3. PRIJAVA PODJETJA (Dodano)
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PodjetjeLoginDto loginData)
        {
            // Poiščemo podjetje po mailu
            var podjetje = await _context.Podjetja
                .FirstOrDefaultAsync(p => p.mail == loginData.Mail);

            // Preverimo če podjetje obstaja in če se geslo ujema
            // POZOR: Preveri če imaš v bazi gesla shranjena z malimi/velikimi črkami (C# loči med njimi!)
            if (podjetje == null || podjetje.geslo != loginData.Geslo)
            {
                return Unauthorized("Napačna e-pošta ali geslo.");
            }

            // Če je vse OK, vrnemo osnovne podatke za localStorage
            return Ok(new {
                id = podjetje.id,
                ime = podjetje.ime,
                mail = podjetje.mail
            });
        }
    }

    // Pomožni razred za prijavo (Dto - Data Transfer Object)
    public class PodjetjeLoginDto
    {
        public string Mail { get; set; } = string.Empty;
        public string Geslo { get; set; } = string.Empty;
    }
}