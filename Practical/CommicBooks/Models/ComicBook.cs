using System.ComponentModel.DataAnnotations;

namespace ComicSystem.Models
{
    public class ComicBook
    {
        [Key]
        public int Id { get; set; }
        public String Title { get; set; }
        public String Author { get; set; }
        public decimal PricePerDay { get; set; }


    }
}
