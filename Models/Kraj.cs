namespace Studentski_servis.Models
{
    public class Kraj
    {
        public int id { get; set; }
        public string ime { get; set; } = string.Empty;
        
        // SPREMENI IZ string V int
        public int postna_st { get; set; } 
    }
}