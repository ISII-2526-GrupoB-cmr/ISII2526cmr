
namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseForCreateDTO
    {

        public PurchaseForCreateDTO() { 
        
        }

        /* ...continuacion punto 5 que está en PurchaseItemDTO.cs...
        ...y solicita al cliente que
        introduzca su nombre, apellidos, dirección y método de pago (Google Pay o Visa) con
        sus datos obligatorios. Para cada coche seleccionado se solicita indicar la cantidad de
        compra, siendo obligatorio.

        */

        public PurchaseForCreateDTO(string customerName, string customerSurname, string deliveryAddress, PaymentMethodTypes paymentMethod, IList<PurchaseItemDTO> purchaseItems)
        {
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            CustomerSurname = customerSurname ?? throw new ArgumentNullException(nameof(customerSurname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            PaymentMethod = paymentMethod;
            
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
        }



        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Display(Name = "Delivery Address")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Delivery address must have at least 10 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]
        public string DeliveryAddress { get; set; }




        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Name")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must have at least 1 characters")]
        public string CustomerName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your Surname")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Surname must have at least 1 characters")]
        public string CustomerSurname { get; set; }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }
        [Required]
        public PaymentMethodTypes PaymentMethod { get; set; }

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseForCreateDTO dTO &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   
                   CustomerName == dTO.CustomerName &&
                   CustomerSurname == dTO.CustomerSurname &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems) &&
                   PaymentMethod == dTO.PaymentMethod;
        }
    }
}