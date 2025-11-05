namespace AppForSEII2526.API.DTOs.RentalDTOs


{
    public class RentalItemDTO
    {
        public RentalItemDTO(int carid, string model, string manufacturer,double rentingPrice )
        {
            CarId = carid;
            Modelo = model;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;

        }

        public int CarId { get; set; }


        public string Modelo { get; set; }


        public double RentingPrice { get; set; }

        public string Manufacturer { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   CarId == dTO.CarId &&
                   Modelo == dTO.Modelo &&
                   RentingPrice == dTO.RentingPrice &&
                   Manufacturer == dTO.Manufacturer;


        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CarId, Modelo, RentingPrice, Manufacturer);
        }
    }
}
