using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your name")]
    public string Name { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your Surname")]
    public string Surname { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please, enter your Address")]
    public string Address { get; set; }

    public IList<Purchase> Purchases { get; set; }

    public IList<Rental> Rentals { get; set; }

    public IList<Review> Reviews { get; set; }


    public ApplicationUser() { 
    }


    public ApplicationUser(string id, string name, string surname, string email, string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        UserName = email;
        Address = address;

    }
}