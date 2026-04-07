using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models;

[Table("podjetja")]
public class Podjetje
{
    [Key]
    public int id { get; set; }
    public string ime { get; set; } = string.Empty;
    public string? opis { get; set; }
    public string mail { get; set; } = string.Empty;
    public int telefon { get; set; }
    public string? spletna_stran { get; set; }
    public int kraji_id { get; set; }
}