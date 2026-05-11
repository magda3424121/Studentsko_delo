using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models
{
    [Table("vrste_uporabnikov")]
    public class VrstaUporabnika
    {
        [Key]
        public int id { get; set; }
        public string ime { get; set; } = string.Empty;
        public string? opis { get; set; }
    }
}