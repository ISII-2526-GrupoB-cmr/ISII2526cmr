namespace AppForSEII2526.API.Models
{
    public class Purchase
    {

        public int Id { get; set; }

        [Display(Name = "Delivery Car Leader")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your address for delivery")]

        public string DeliveryCarLeader { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethodTypes PaymentMethod { get; set; }

        public DateTime PurchasingDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please, set your purchasing type")]
        public string PurchasingType  { get; set; }

        public IList<PurchaseItem> PurchaseItems { get; set; }


    }

    public enum PaymentMethodTypes
    {
        GooglePay,
        Visa
    }
}
