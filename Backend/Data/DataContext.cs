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

        // DODAJ TA DEL SPODAJ:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // To prisili Entity Framework, da uporabi mala imena tabel, 
            // ki se ujemajo s tistimi v Neon bazi
            modelBuilder.Entity<Kraj>().ToTable("kraji");
            modelBuilder.Entity<Podjetje>().ToTable("podjetja");
            modelBuilder.Entity<Uporabnik>().ToTable("uporabniki");
            modelBuilder.Entity<VrstaUporabnika>().ToTable("vrste_uporabnikov");
            modelBuilder.Entity<Objava>().ToTable("objave");
            modelBuilder.Entity<Prijava>().ToTable("prijave");
        }
    }
}