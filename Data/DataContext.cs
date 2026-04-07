using Microsoft.EntityFrameworkCore;
using Studentski_servis.Models;

namespace Studentski_servis.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Podjetje> Podjetja { get; set; }
        public DbSet<Uporabnik> Uporabniki { get; set; }
        public DbSet<VrstaUporabnika> VrsteUporabnikov { get; set; }
        public DbSet<Objava> Objave { get; set; }
        public DbSet<Kraj> Kraji { get; set; }
        public DbSet<Prijava> Prijave { get; set; }
    }
}