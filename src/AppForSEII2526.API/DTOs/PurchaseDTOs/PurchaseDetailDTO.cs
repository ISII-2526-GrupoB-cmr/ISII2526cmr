using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {

        public PurchaseDetailDTO () 
        {

        }  
        public PurchaseDetailDTO(int id, DateTime purchaseDate, string customerName, string customerSurname,
            string deliveryAddress, float totalPrice, IList<PurchaseItemDTO> purchaseItems)
        {
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            CustomerSurname = customerSurname ?? throw new ArgumentNullException(nameof(customerSurname));
            DeliveryAddress = deliveryAddress ?? throw new ArgumentNullException(nameof(deliveryAddress));
            TotalPrice = totalPrice;
            PurchaseItems = purchaseItems ?? throw new ArgumentNullException(nameof(purchaseItems));
            Id = id;
            PurchaseDate = purchaseDate;
        }
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string CustomerName { get; set; }

        public string CustomerSurname { get; set; }

        public string DeliveryAddress { get; set; }

        public float TotalPrice { get; set; }

        public IList<PurchaseItemDTO> PurchaseItems { get; set; }


        

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, PurchaseDate);
        }

        public bool CompareDate(DateTime date1, DateTime date2)
        {
            return date1.Date == date2.Date;
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   PurchaseDate == dTO.PurchaseDate &&
                   CustomerName == dTO.CustomerName &&
                   CustomerSurname == dTO.CustomerSurname &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   TotalPrice == dTO.TotalPrice &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems);
        }
    }
}