namespace AppForSEII2526.API.Models
{
    public class Purchase
    {

        public int Id { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Delivery Car Dealer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your delivery car Dealer")]
        public string DeliveryCarLeader { get; set; }

        [Display(Name = "Payment Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your payment method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        [Display(Name = "Purchasing Date")]
        public DateTime PurchasingDate { get; set; }

      
        [Display(Name = "Purchasing Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your purchasing price")]
        public float PurchasingPrice  { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }


    }

    public enum PaymentMethodTypes
    {
        GooglePay,
        Paypal,
        Visa
    }
}
