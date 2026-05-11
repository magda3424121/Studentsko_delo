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

    [HttpGet("podjetje/{podjetjeId}")]
    public async Task<IActionResult> GetPrijaveZaPodjetje(int podjetjeId)
    {
        // Podjetje potrebuje razširjen pogled: podatke o prijavi, oglasu in kandidatu v enem odgovoru.
        var prijave = await (
            from prijava in _context.Prijave
            join objava in _context.Objave on prijava.objave_id equals objava.id
            join uporabnik in _context.Uporabniki on prijava.uporabniki_id equals uporabnik.id
            where objava.podjetja_id == podjetjeId
            orderby prijava.datum_prijave descending
            select new
            {
                id = prijava.id,
                datum_prijave = prijava.datum_prijave,
                status = prijava.status,
                objave_id = objava.id,
                naziv_dela = objava.ime,
                lokacija = objava.lokacija,
                placa = objava.placa,
                uporabniki_id = uporabnik.id,
                ime = uporabnik.ime,
                priimek = uporabnik.priimek,
                mail = uporabnik.mail,
                telefon = uporabnik.telefon,
                sola = uporabnik.sola
            }).ToListAsync();

        return Ok(prijave);
    }

    [HttpGet("uporabnik/{uporabnikId}")]
    public async Task<IActionResult> GetPrijaveZaUporabnika(int uporabnikId)
    {
        // Študent vidi zgodovino svojih prijav skupaj s trenutnim statusom odločitve podjetja.
        var prijave = await (
            from prijava in _context.Prijave
            join objava in _context.Objave on prijava.objave_id equals objava.id
            join podjetje in _context.Podjetja on objava.podjetja_id equals podjetje.id
            where prijava.uporabniki_id == uporabnikId
            orderby prijava.datum_prijave descending
            select new
            {
                id = prijava.id,
                datum_prijave = prijava.datum_prijave,
                status = prijava.status,
                objave_id = objava.id,
                naziv_dela = objava.ime,
                lokacija = objava.lokacija,
                placa = objava.placa,
                podjetje = podjetje.ime
            }).ToListAsync();

        return Ok(prijave);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Prijava>> GetPrijava(int id)
    {
        var prijava = await _context.Prijave.FindAsync(id);
        if (prijava == null) return NotFound();

        return Ok(prijava);
    }

    [HttpPost]
    public async Task<ActionResult<Prijava>> PostPrijava(Prijava prijava)
    {
        // En uporabnik se na isti oglas lahko prijavi samo enkrat.
        var obstojecaPrijava = await _context.Prijave.FirstOrDefaultAsync(p =>
            p.uporabniki_id == prijava.uporabniki_id && p.objave_id == prijava.objave_id);

        if (obstojecaPrijava != null)
        {
            return Conflict(new { message = "Na ta oglas ste se že prijavili." });
        }

        prijava.datum_prijave = DateTime.UtcNow;
        if (string.IsNullOrWhiteSpace(prijava.status)) prijava.status = "V obravnavi";
        
        _context.Prijave.Add(prijava);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetPrijava), new { id = prijava.id }, prijava);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPrijava(int id, Prijava prijava)
    {
        if (id != prijava.id) return BadRequest();

        _context.Entry(prijava).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> PutStatus(int id, [FromBody] PrijavaStatusDto statusDto)
    {
        // Status je omejen na tri vrednosti, da frontend in baza ostaneta usklajena.
        var prijava = await _context.Prijave.FindAsync(id);
        if (prijava == null) return NotFound();

        if (statusDto.Status != "Sprejeta" && statusDto.Status != "Zavrnjena" && statusDto.Status != "V obravnavi")
        {
            return BadRequest(new { message = "Neveljaven status prijave." });
        }

        prijava.status = statusDto.Status;
        await _context.SaveChangesAsync();

        return Ok(prijava);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrijava(int id)
    {
        var prijava = await _context.Prijave.FindAsync(id);
        if (prijava == null) return NotFound();

        _context.Prijave.Remove(prijava);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

public class PrijavaStatusDto
{
    public string Status { get; set; } = "V obravnavi";
}
