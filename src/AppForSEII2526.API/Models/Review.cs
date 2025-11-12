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

        public String country { get; set; }
        public DateTime created { get; set; }

        public DriverType drivertype { get; set; } //Ahora es un enum y solo acepta "novato" o "experto"

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }

        public Review() { }

        public Review(string country, string username, string manufacturer, string color, int rating, string description, string model, DriverType drivertype, List<ReviewItem> reviewItems, string fueltype)
        {
            this.country = country;
            this.created = DateTime.Now;
            this.drivertype = drivertype;
            ReviewItems = reviewItems;
            

        }
    }


}
