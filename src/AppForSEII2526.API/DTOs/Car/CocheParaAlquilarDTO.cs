namespace AppForSEII2526.API.DTOs.Car
{
    public class CocheParaAlquilarDTO
    {
        public int Id { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string FuelType { get; set; }

        public string Manufacturer { get; set; }

        public float RentingPrice { get; set; }

        public CocheParaAlquilarDTO(int id, string model, string color, string fuelType, string manufacturer, float rentingPrice)
        {
            Id = id;
            Model = model;
            Color = color;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
        }

        public override bool Equals(object? obj)
        {
            return obj is CocheParaAlquilarDTO dTO &&
                   Id == dTO.Id &&
                   Model == dTO.Model &&
                   Color == dTO.Color &&
                   FuelType == dTO.FuelType &&
                   Manufacturer == dTO.Manufacturer &&
                   RentingPrice == dTO.RentingPrice;
        }
    }
}