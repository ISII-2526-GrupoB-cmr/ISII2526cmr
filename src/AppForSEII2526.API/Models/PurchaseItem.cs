
namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(PurchaseId))]

    public class PurchaseItem
    {
        public int CarId { get; set; }
        public int PurchaseId { get; set; }

        public int Quantity { get; set; }

        public Car Car { get; set; }
        public Purchase Purchase { get; set; }

    }
}