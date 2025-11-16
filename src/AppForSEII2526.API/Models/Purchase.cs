

namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        private ApplicationUser? user;
        private int quantity;

        public Purchase ()
        {

        }
        public Purchase(string customerName, string customerSurname, int quantity, string deliveryAddress, PaymentMethodTypes paymentMethod, List<PurchaseItem> purchaseItems)
        {
            CustomerName = customerName;
            CustomerSurname = customerSurname;
            this.quantity = quantity;
            DeliveryAddress = deliveryAddress;
            PaymentMethod = paymentMethod;
            PurchaseItems = purchaseItems;
            DeliveryCarDealer = "Concesionario Albacete";
            PurchasingDate = DateTime.Now;
            PurchasingPrice = 0;
        }

        public Purchase(ApplicationUser user, string deliveryCarDealer, PaymentMethodTypes paymentMethod, DateTime purchasingDate, float purchasingPrice, IList<PurchaseItem> purchaseItems)
        {
            ApplicationUser = user;
            DeliveryCarDealer = deliveryCarDealer;
            PaymentMethod = paymentMethod;
            PurchasingDate = purchasingDate;
            PurchasingPrice = purchasingPrice;
            PurchaseItems = purchaseItems;
        }

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Delivery Car Dealer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your delivery car Dealer")]
        public string DeliveryCarDealer { get; set; }

        [Display(Name = "Payment Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your payment method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [Display(Name = "Purchasing Date")]
        public DateTime PurchasingDate { get; set; }

      
        [Display(Name = "Purchasing Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your purchasing price")]
        public float PurchasingPrice { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }
        public string CustomerName { get; }
        public string CustomerSurname { get; }
        public string DeliveryAddress { get; }
    }


    

public enum PaymentMethodTypes
    {
        GooglePay,
        Paypal,
        Visa
    }
}
