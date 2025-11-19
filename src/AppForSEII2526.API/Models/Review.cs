using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppForSEII2526.API.Models
{
    public enum DriverType
    {
        novato,
        experto
    }
    public class Review
    {

        public String Country { get; set; }
        public DateTime Created { get; set; }

        public DriverType Drivertype { get; set; } //Ahora es un enum y solo acepta "novato" o "experto"

        [Key]
        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }

        public Review() { }


        public Review(string country, DateTime created, ApplicationUser applicationUser, DriverType drivertype, IList<ReviewItem> reviewItems)
        {
            Country = country;
            Created = created;
            ApplicationUser = applicationUser;
            Drivertype = drivertype;
            ReviewItems = reviewItems;

        }
    }


}
