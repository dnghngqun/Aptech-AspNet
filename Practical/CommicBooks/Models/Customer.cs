using System.ComponentModel.DataAnnotations;

namespace ComicSystem.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public String Fullname { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
