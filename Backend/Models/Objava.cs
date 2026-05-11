using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models;

[Table("objave")]
public class Objava
{
    [Key]
    public int id { get; set; }
    public string ime { get; set; } = string.Empty;
    public string opis { get; set; } = string.Empty;
    public double placa { get; set; }
    public string delovnik { get; set; } = string.Empty;
    public string trajanje { get; set; } = string.Empty;
    public int podjetja_id { get; set; }
    public string? lokacija { get; set; }
    public int? kraj_id { get; set; }
}