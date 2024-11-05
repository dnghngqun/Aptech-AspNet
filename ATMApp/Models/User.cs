using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace ATMApp.Models{
    public class User{
        [Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        
        public List<Account>? Accounts { get; set; }
        }

    
}