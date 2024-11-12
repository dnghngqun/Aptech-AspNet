using ComicSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ComicSystem.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ComicSystemContext _context;

        public ReportController(ComicSystemContext context)
        {
            _context = context;
        }

        // GET: api/report?startDate=yyyy-MM-dd&endDate=yyyy-MM-dd
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentalsReport(DateTime startDate, DateTime endDate)
        {
            var rentals = await _context.Rentals
                .Where(r => r.RentalDate >= startDate && r.RentalDate <= endDate)
                .Include(r => r.RentalDetails)
                .ThenInclude(rd => rd.ComicBook)
                .Include(r => r.Customer)
                .ToListAsync();

            if (rentals == null || rentals.Count == 0)
            {
                return NotFound("No rentals found for the specified date range.");
            }

            // Format the result for reporting purposes (optional)
            var reportData = rentals.Select(r => new
            {
                BookName = r.RentalDetails.Select(rd => rd.ComicBook.Title),
                RentalDate = r.RentalDate,
                ReturnDate = r.ReturnDate,
                CustomerName = r.Customer.Fullname,
                Quantity = r.RentalDetails.Select(rd => rd.Quantity),
            }).ToList();

            return Ok(reportData);
        }
    }
}
