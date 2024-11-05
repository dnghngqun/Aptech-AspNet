using ATMApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMApp.Data{
public class ATMContext : DbContext
    {
        public ATMContext(DbContextOptions<ATMContext> options) : base(options){}
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<User>()
           .HasMany(u => u.Accounts)
           .WithOne(a => a.User) // One user has many accounts
           .HasForeignKey(a => a.UserId);// Foreign key in Account table


           modelBuilder.Entity<Account>()
           .HasMany(a => a.Transactions)
           .WithOne(t => t.Account)
           .HasForeignKey(t => t.AccountId);
        }


    }
}