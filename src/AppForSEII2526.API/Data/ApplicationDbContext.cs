using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

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

   

    

