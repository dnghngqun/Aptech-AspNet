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
        public async Task<ActionResult<Rental>> PostRental(RentalCreateDTO rentalDTO)
        {
            if (rentalDTO.RentalDetails == null || rentalDTO.RentalDetails.Count == 0)
            {
                return BadRequest("At least one rental detail is required.");
            }

            // Tạo đối tượng Rental từ DTO
            Rental rental = new Rental
            {
                CustomerId = rentalDTO.CustomerId,
                RentalDate = rentalDTO.RentalDate,
                ReturnDate = rentalDTO.ReturnDate,
                Status = Rental.RentalStatus.Rented, // Tự động set Status là Rented
                RentalDetails = rentalDTO.RentalDetails.Select(rd => new RentalDetail
                {
                    ComicBookId = rd.ComicBookId,
                    Quantity = rd.Quantity
                }).ToList()
            };

            // Thêm rental vào database
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            foreach (var rentalDetail in rental.RentalDetails)
            {
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
        // DTO cho Rental
    public class RentalCreateDTO
    {
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public List<RentalDetailDTO> RentalDetails { get; set; }
    }

    // DTO cho RentalDetail
    public class RentalDetailDTO
    {
        public int ComicBookId { get; set; }
        public int Quantity { get; set; }
    }

}
