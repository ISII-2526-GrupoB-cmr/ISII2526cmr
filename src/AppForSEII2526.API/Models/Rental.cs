

namespace AppForSEII2526.API.Models
{
    public class Rental
    {
       

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Delivery Car Dealer")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        public string DeliveryCarDealer { get; set; }
       
        public int Id { get; set; }


        [Display(Name = "Payment Method")]
        [StringLength(15, ErrorMessage = "Name can be neither longer than 50 characters nor shorter than 10.", MinimumLength=3)]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [Display(Name = "Renting Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RentignDate { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Total Price")]
        [Precision(10,2)]
        public double TotalPrice { get; set; }

        public Rental() { }
        public Rental(string deliveryCarDealer, string surname, PaymentMethodTypes paymentMethod, DateTime rentignDate, DateTime startDate, DateTime endDate, double totalPrice)
        {
            DeliveryCarDealer = deliveryCarDealer;
            PaymentMethod = (PaymentMethodTypes)paymentMethod;
            RentignDate = rentignDate;
            StartDate = startDate;
            EndDate = endDate;
            TotalPrice = totalPrice;
        }

        public Rental(string name, string surname, string address, DateTime now, PaymentMethodTypes paymentMethod, DateTime startDate, DateTime endDate, List<RentalItem> rentalItems)
        {
            Name = name;
            Surname = surname;
            Address = address;
            Now = now;
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentalItems = rentalItems;
        }

        [Display(Name = "List of Rental Items")]

        public IList<RentalItem> RentalItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string Name { get; }
        public string Surname { get; }
        public string Address { get; }
        public DateTime Now { get; }
    }
    
}