namespace AppForSEII2526.API.DTOs.RentalDTOs


{
    public class RentalItemDTO
    {
        public RentalItemDTO(int carid, string model, string manufacturer,double rentingPrice)
        {
            CarId = carid;
            Model = model;
            Manufacturer = manufacturer;
            RentingPrice = rentingPrice;
        }

        public int CarId { get; set; }


        public string Model { get; set; }


        public double RentingPrice { get; set; }

        public string Manufacturer { get; set; }


        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   CarId == dTO.CarId &&
                   Model == dTO.Model &&
                   RentingPrice == dTO.RentingPrice &&
                   Manufacturer == dTO.Manufacturer;
                   
                  
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CarId, Model, RentingPrice, Manufacturer);
        }
    }
}
