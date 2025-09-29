using Microsoft.AspNetCore.Identity;

namespace AppForSEII2526.API.Models;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser {
}

public class Review { 
        public String country { get; set; }
        public DateTime created { get; set; }

       public String drivertype { get; set; }

       public int Id { get; set; }

         public String UserName { get; set; }
}

