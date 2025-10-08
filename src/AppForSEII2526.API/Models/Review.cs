namespace AppForSEII2526.API.Models
{
    public enum DriverType
    {
        novato,
        experto
    }
    public class Review
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]

        public String country { get; set; }
        public DateTime created { get; set; }

        public DriverType drivertype { get; set; } //Ahora es un enum y solo acepta "novato" o "experto"

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public IList<ReviewItem> ReviewItems { get; set; }
    }

}
