namespace AppForSEII2526.API.DTOs.RentalDTOs


{
    public class RentalItemDTO
    {
        public RentalItemDTO(int carId, string modelo, string manufacturer, double rentingPrice,int quantity)
        {
            CarId = carId;
            Modelo = modelo;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
            Quantity = quantity;
        }


        public int CarId { get; set; }


        public string Modelo { get; set; }
        public int Quantity { get; set; }




        public double RentingPrice { get; set; }

        public string Manufacturer { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   CarId == dTO.CarId &&
                   Modelo == dTO.Modelo &&
                   RentingPrice == dTO.RentingPrice &&
                   Manufacturer == dTO.Manufacturer &&
                   Quantity == dTO.Quantity;


        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CarId, Modelo, RentingPrice, Manufacturer,Quantity);
        }
    }
}
