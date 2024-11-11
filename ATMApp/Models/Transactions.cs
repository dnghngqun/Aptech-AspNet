using System.ComponentModel.DataAnnotations;

namespace ATMApp.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int AccountId { get; set; }
        
        public Account? Account { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool IsSuccessful { get; set; }
        public string Description { get; set; } 
    }
}