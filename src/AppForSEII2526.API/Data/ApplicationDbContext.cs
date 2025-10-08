using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {

    DbSet<Model> Models { get; set; }
    DbSet<Purchase> Purchases { get; set; }
    DbSet<PurchaseItem> PurchaseItems { get; set; }
    DbSet<ReviewItem> ReviewItems { get; set; }
    DbSet<Car> Cars { set; get; }
    DbSet<Review> Reviews { get; set; }
    DbSet<RentalItem> RentalItems { get; set; }
    DbSet<Rental> Rentals { get; set; }
    DbSet<ApplicationUser> ApplicationUsers { get; set; }
}

   

    

