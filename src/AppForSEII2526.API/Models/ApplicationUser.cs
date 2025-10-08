using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {


    public string UserName { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public IList<Purchase> Purchases { get; set; }

    public IList<Rental> Rentals { get; set; }

    public IList<Review> Reviews { get; set; }
}

