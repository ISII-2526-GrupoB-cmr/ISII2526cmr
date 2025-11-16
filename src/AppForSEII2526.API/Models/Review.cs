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

        public ApplicationUser Applicationuser { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }

        public Review() { }

        public Review(string country, DateTime created, ApplicationUser applicationUser, DriverType drivertype, IList<ReviewItem> reviewItems)
        {
            Country = country;
            Created = created;
            Applicationuser = applicationUser;
            Drivertype = drivertype;
            ReviewItems = reviewItems;

        }
    }


}
