using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace ATMApp.Models{
    public class Account{
        [Key]
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public User? User {get; set;}
        public decimal Balance { get; set; }
        public AccountType AccountType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public float? InterestRate { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }

    public enum AccountType{
        Checking,
        Savings,
        Credit
    }

}