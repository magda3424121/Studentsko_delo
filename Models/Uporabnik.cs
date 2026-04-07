using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models;

[Table("uporabniki")]
public class Uporabnik
{
    [Key]
    public int id { get; set; }
    public string ime { get; set; } = string.Empty; // Pazi: v tvojem SQL je bil int, tukaj popravljamo na string
    public string priimek { get; set; } = string.Empty;
    public int telefon { get; set; }
    public string mail { get; set; } = string.Empty;
    public string? sola { get; set; }
    public string geslo { get; set; } = string.Empty;
    public string cv { get; set; } = string.Empty;
    public int vrste_uporabnikov_id { get; set; }
    public int kraji_id { get; set; }
}