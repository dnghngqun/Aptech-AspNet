using Microsoft.EntityFrameworkCore;

namespace ComicSystem.Models
{
    public class ComicSystemContext : DbContext
    {
        public ComicSystemContext(DbContextOptions<ComicSystemContext> options) : base(options) { }

        // DbSets for all models
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<RentalDetail> RentalDetails { get; set; }

        // Optional: Configure relationships, keys, etc. if needed
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring primary keys and relationships if needed
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Customer)
                .WithMany() // Assuming no navigation property on Customer for Rentals
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // If Rental is deleted, its related RentalsDetails should be deleted

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.Rental)
                .WithMany(r => r.RentalDetails) // This should be configured for the relationship from Rental
                .HasForeignKey(rd => rd.RentalId)
                .OnDelete(DeleteBehavior.Cascade); // Delete related RentalDetails if Rental is deleted

            modelBuilder.Entity<RentalDetail>()
                .HasOne(rd => rd.ComicBook)
                .WithMany() // Assuming no navigation property on ComicBook for RentalDetails
                .HasForeignKey(rd => rd.ComicBookId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting ComicBook if it's in a RentalDetail

            base.OnModelCreating(modelBuilder);
        }
    }
}
