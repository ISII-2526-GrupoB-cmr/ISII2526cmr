using Humanizer;

namespace AppForSEII2526.API.DTOs.RentalDTOs

{
    public class RentalForCreateDTO
    {
        public RentalForCreateDTO(string name, string surname,int quantity, string address, PaymentMethodTypes paymentMethod, DateTime startDate, DateTime endDate, IList<RentalItemDTO> rentalItems)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentalItems = rentalItems ?? throw new ArgumentNullException(nameof(rentalItems));
            Quantity = quantity;
        }

        public RentalForCreateDTO()
        {
            RentalItems = new List<RentalItemDTO>();
        }
        public int Quantity { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string Address { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name must have at least 2 characters")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name and Surname")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Name and Surname must have at least 10 characters")]
        public string Surname { get; set; }

        public IList<RentalItemDTO> RentalItems { get; set; }
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        private int NumberOfDays
        {
            get
            {
                return (EndDate - StartDate).Days;
            }
        }

        [Display(Name = "Total Price")]
        [JsonPropertyName("TotalPrice")]
        public double TotalPrice
        {
            get
            {
                return RentalItems.Sum(ri => ri.RentingPrice * NumberOfDays);
            }
        }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is RentalForCreateDTO dTO &&
                   CompareDate(StartDate, dTO.StartDate) &&
                   CompareDate(EndDate, dTO.EndDate) &&
                   Address == dTO.Address &&
                   Name == dTO.Name &&
                   Surname == dTO.Surname &&
                   RentalItems.SequenceEqual(dTO.RentalItems) &&
                   PaymentMethod == dTO.PaymentMethod &&
                   TotalPrice == dTO.TotalPrice;
        }
    }
}
