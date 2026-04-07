using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Studentski_servis.Models
{
    [Table("kraji")]
    public class Kraj
    {
        [Key]
        public int id { get; set; }
        public string ime { get; set; } = string.Empty;
        public int postna_st { get; set; }
    }
}