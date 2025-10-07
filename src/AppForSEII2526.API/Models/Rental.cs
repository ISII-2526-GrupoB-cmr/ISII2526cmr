namespace AppForSEII2526.API.Models
{
    public class Rental
    {
        public Rental()
        {

        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Delivery Car Dealer")]

        public string DeliveryCarDealer { get; set; }
       
        public int Id { get; set; }


        [Display(Name = "Payment Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your PaymentMethod")]
        public string PaymentMethod { get; set; }

        [Display(Name = "Renting Date")]

        public DateTime RentignDate { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Total Price")]

        public double TotalPrice { get; set; }
        [Display(Name = "List of Rental Items")]

        public IList<RentalItem> RentalItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
    public enum PaymentMethod
    {
        Visa,
        GooglePay,
        Paypal

    }
}