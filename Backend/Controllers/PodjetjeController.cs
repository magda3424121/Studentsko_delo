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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Podjetje>>> GetPodjetja()
        {
            return await _context.Podjetja.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Podjetje>> GetPodjetje(int id)
        {
            var podjetje = await _context.Podjetja.FindAsync(id);
            if (podjetje == null) return NotFound();

            return Ok(podjetje);
        }

        [HttpGet("kraji")]
        public async Task<IActionResult> GetKraji()
        {
            var kraji = await _context.Kraji.OrderBy(k => k.ime).ToListAsync();
            return Ok(kraji);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Podjetje>> PostPodjetje([FromBody] Podjetje podjetje)
        {
            try 
            {
                _context.Podjetja.Add(podjetje);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPodjetje), new { id = podjetje.id }, podjetje);
            }
            catch (Exception ex)
            {
                return BadRequest("Napaka pri registraciji: " + (ex.InnerException?.Message ?? ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPodjetje(int id, Podjetje podjetje)
        {
            if (id != podjetje.id) return BadRequest();

            _context.Entry(podjetje).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePodjetje(int id)
        {
            var podjetje = await _context.Podjetja.FindAsync(id);
            if (podjetje == null) return NotFound();

            _context.Podjetja.Remove(podjetje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] PodjetjeLoginDto loginData)
        {
            if (loginData == null || string.IsNullOrEmpty(loginData.Mail))
            {
                return BadRequest("Podatki niso pravilno poslani.");
            }

            // 1. Poiščemo podjetje (uporabimo .ToLower(), da preprečimo težave z velikimi črkami pri mailu)
            var podjetje = await _context.Podjetja
                .FirstOrDefaultAsync(p => p.mail.ToLower() == loginData.Mail.ToLower());

            // 2. Preverimo geslo (C# gesla primerja natančno - "Geslo123" ni isto kot "geslo123")
            if (podjetje == null || podjetje.geslo != loginData.Geslo)
            {
                return Unauthorized(new { message = "Napačna e-pošta ali geslo." });
            }

            // 3. VRNEMO PODATKE
            // Pomembno: Vrnemo objekt, ki ga JavaScript pričakuje (pazi na male črke v ključih!)
            return Ok(new {
                id = podjetje.id,
                ime = podjetje.ime,
                mail = podjetje.mail
            });
        }
    }

    public class PodjetjeLoginDto
    {
        public string Mail { get; set; } = string.Empty;
        public string Geslo { get; set; } = string.Empty;
    }
}
