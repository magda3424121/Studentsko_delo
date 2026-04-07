using Microsoft.EntityFrameworkCore;
using Studentski_servis.Models; // To bova potrebovala za tabele

namespace Studentski_servis.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<JobOffer> JobOffers { get; set; }
    }
}