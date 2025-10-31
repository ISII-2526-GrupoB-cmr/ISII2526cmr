
namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(PurchaseId))]

    public class PurchaseItem
    {
        public int CarId { get; set; }
        public int PurchaseId { get; set; }

        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        public Car Car { get; set; }
        public Purchase Purchase { get; set; }

        // Campos auxiliares no mapeados a la BD para mantener compatibilidad
        [NotMapped]
        public float PurchasePrice { get; set; }

        [NotMapped]
        public float TotalPrice { get; set; }

        [NotMapped]
        public string CarColor { get; set; }

        [NotMapped]
        public string? Description { get; set; }

        // Constructor usado en PurchasesController:
        // new PurchaseItem(car.Id, purchase, car.PurchasePrice, purchase.PurchasingPrice, item.CarColor, item.Description)
        public PurchaseItem(int carId, Purchase purchase, float purchasePrice, float totalPrice, string carColor, string? description = "")
        {
            CarId = carId;
            Purchase = purchase;
            PurchaseId = purchase?.Id ?? 0;
            Quantity = 1;

            PurchasePrice = purchasePrice;
            TotalPrice = totalPrice;
            CarColor = carColor;
            Description = description;
        }

        // Constructor usado en pruebas y lugares donde se dispone del objeto Car:
        // new PurchaseItem(car, purchase, car.PurchasePrice)
        public PurchaseItem(Car car, Purchase purchase, float purchasePrice)
        {
            Car = car;
            CarId = car?.Id ?? 0;
            Purchase = purchase;
            PurchaseId = purchase?.Id ?? 0;
            Quantity = 1;

            PurchasePrice = purchasePrice;
            TotalPrice = purchasePrice;
            CarColor = car?.Color;
            Description = car?.Description;
        }

        // Constructor por defecto requerido por EF
        public PurchaseItem() { }
    }
}