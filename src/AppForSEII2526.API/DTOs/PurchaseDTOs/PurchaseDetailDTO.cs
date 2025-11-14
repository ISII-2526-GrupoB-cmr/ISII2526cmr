using AppForSEII2526.API.Models;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppForSEII2526.API.DTOs.PurchaseDTOs
{
    public class PurchaseDetailDTO
    {

        public PurchaseDetailDTO () 
        {

        }  

        /*
        7. El sistema muestra la compra realizada indicando los datos del cliente(nombre y
        apellidos), dirección, cuando se realizó la compra, el precio total y los coches
        comprados(modelo, precio, color y cantidad).
        */

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

        protected bool CompareDate(DateTime date1, DateTime date2)
        {
            return (date1.Subtract(date2) < new TimeSpan(0, 1, 0));
        }

        public override bool Equals(object? obj)
        {
            return obj is PurchaseDetailDTO dTO &&
                   Id == dTO.Id &&
                   CompareDate(dTO.PurchaseDate, PurchaseDate) && //permite un rango de diferencia de 1 minuto
                   //PurchaseDate == dTO.PurchaseDate &&
                   CustomerName == dTO.CustomerName &&
                   CustomerSurname == dTO.CustomerSurname &&
                   DeliveryAddress == dTO.DeliveryAddress &&
                   TotalPrice == dTO.TotalPrice &&
                   PurchaseItems.SequenceEqual(dTO.PurchaseItems);
        }
    }
}