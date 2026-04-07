using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models
{
    [Table("objave")]
    public class Objava
    {
        [Key]
        public int id { get; set; }
        
        // V SQL-u si imela 'ime' kot integer, kar je verjetno napaka. 
        // Če si v Neonu spremenila v text, uporabi string.
        public string ime { get; set; } = string.Empty; 
        
        public string opis { get; set; } = string.Empty;
        public double placa { get; set; }
        public string delovnik { get; set; } = string.Empty;
        public string trajanje { get; set; } = string.Empty;
        public int podjetja_id { get; set; }
    }
}