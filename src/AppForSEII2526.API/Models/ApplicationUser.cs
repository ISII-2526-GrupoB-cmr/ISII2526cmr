using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {

}

public class Model {
    public int ID { get; set; };
    public string Name { get; set; };
}

