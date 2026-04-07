using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Studentski_servis.Data;
using Studentski_servis.Models;

namespace Studentski_servis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferController : ControllerBase
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
        }

        // Pridobi vse oglase
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobOffer>>> GetJobs()
        {
            return await _context.JobOffers.ToListAsync();
        }

        // Dodaj nov oglas
        [HttpPost]
        public async Task<ActionResult<JobOffer>> PostJob(JobOffer job)
        {
            _context.JobOffers.Add(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
    }
}