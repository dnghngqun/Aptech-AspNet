using System.ComponentModel.DataAnnotations;

namespace ComicSystem.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public RentalStatus Status { get; set; }
        public ICollection<RentalDetail> RentalDetails { get; set; } 
        public enum RentalStatus
        {
            Pending,
            Rented,
            Available,
            Returned
        }

        public Customer Customer { get; set; }
    }
}
