using System.ComponentModel.DataAnnotations;

namespace Studentski_servis.Models
{
    public class JobOffer
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double HourlyRate { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}