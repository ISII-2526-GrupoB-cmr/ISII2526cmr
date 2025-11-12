
namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {

        public PurchaseForCreateDTO() { 
        
        }

        public PurchaseForCreateDTO(string customerName, string customerSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, int quantity, IList<PurchaseItemDTO> purchaseItems)
        {
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            CustomerSurname = customerSurname ?? throw new ArgumentNullException(nameof(customerSurname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            Quantity = quantity;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
        }



        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must have at least 1 characters")]
        public string CustomerName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Surname")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Surname must have at least 1 characters")]
        public string CustomerSurname { get; set; }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   Quantity == dTO.Quantity &&
                   CustomerName == dTO.CustomerName &&
                   CustomerSurname == dTO.CustomerSurname &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems) &&
                   PaymentMethod == dTO.PaymentMethod;
        }
    }
}