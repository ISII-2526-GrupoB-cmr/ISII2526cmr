namespace AppForSEII2526.API.Models
{
    public class Purchase
    {
        public int DeliveryCarLeader { get; set; }
        
        public int Id { get; set; }

        public string Name { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime PurchasingDate { get; set; }

        public string PurchasingType  { get; set; }

        public string Surname { get; set; }
    }
}
