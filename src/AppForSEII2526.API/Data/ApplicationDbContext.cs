using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // clave compuesta
        builder.Entity<RentalItem>().HasKey(pi => new { pi.CarId, pi.RentalId });

        // relación RentalItem -> Rental (many)
        builder.Entity<RentalItem>()
            .HasOne(ri => ri.Rental)
            .WithMany(r => r.RentalItems)
            .HasForeignKey(ri => ri.RentalId)
            .OnDelete(DeleteBehavior.Cascade);

        // relación RentalItem -> Car (many)
        builder.Entity<RentalItem>()
            .HasOne(ri => ri.Car)
            .WithMany(c => c.RentalItems)
            .HasForeignKey(ri => ri.CarId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<Model> Models { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
    public DbSet<PurchaseItem> PurchaseItems { get; set; }
    public DbSet<ReviewItem> ReviewItems { get; set; }
    public DbSet<Car> Cars { set; get; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<RentalItem> RentalItems { get; set; }
    public DbSet<Rental> Rentals { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

}

