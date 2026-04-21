public class Podjetje
{
    public int id { get; set; }
    public string ime { get; set; } = string.Empty; 
    public string? opis { get; set; } 
    public string mail { get; set; } = string.Empty; 
    public string telefon { get; set; } = string.Empty; 
    public string? spletna_stran { get; set; }
    public string geslo { get; set; } = string.Empty; 
    public int kraji_id { get; set; }
    public int vrsta_uporabnika_id { get; set; } = 2;
}