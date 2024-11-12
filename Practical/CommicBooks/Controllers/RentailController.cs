using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Controllers
{
    [Route("api/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public RentalsController(ComicSystemContext context)
        {
            _context = context;
        }

        // POST: api/rentals
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            // Calculate total price based on rented books and quantity
            rental.Status = Rental.RentalStatus.Rented; // Mark rental status as rented

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            foreach (var rentalDetail in rental.RentalDetails)
            {
                // Set price per day for each rental detail
                rentalDetail.PricePerDay = _context.ComicBooks
                    .Where(cb => cb.Id == rentalDetail.ComicBookId)
                    .Select(cb => cb.PricePerDay)
                    .FirstOrDefault();
                
                _context.RentalDetails.Add(rentalDetail);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRental), new { id = rental.Id }, rental);
        }

        // GET: api/rentals/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals.Include(r => r.RentalDetails)
                                                .ThenInclude(rd => rd.ComicBook)
                                                .FirstOrDefaultAsync(r => r.Id == id);

            if (rental == null)
            {
                return NotFound();
            }

            return rental;
        }
    }
}
