namespace AppForSEII2526.API.DTOs.Car
{
    public class CocheParaComprarDTO
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string FuelType { get; set; }

        public string Manufacturer { get; set; }

        public float PurchasePrice { get; set; }

        public CocheParaComprarDTO(int id, string model, string color, string fuelType, string manufacturer, float purchasePrice)
        {
            Id = id;
            Model = model;
            Color = color;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            PurchasePrice = purchasePrice;
        }

        public override bool Equals(object? obj)
        {
            return obj is CocheParaComprarDTO dTO &&
                   Id == dTO.Id &&
                   Model == dTO.Model &&
                   Color == dTO.Color &&
                   FuelType == dTO.FuelType &&
                   Manufacturer == dTO.Manufacturer &&
                   PurchasePrice == dTO.PurchasePrice;
        }
    }
}
