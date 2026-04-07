using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models
{
    [Table("prijave")]
    public class Prijava
    {
        [Key]
        public int id { get; set; }
        public DateTime datum_prijave { get; set; }
        public int uporabniki_id { get; set; }
        public int objave_id { get; set; }
    }
}