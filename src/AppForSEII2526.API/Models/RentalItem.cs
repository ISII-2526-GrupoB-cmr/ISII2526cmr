namespace AppForSEII2526.API.Models { 
[PrimaryKey(nameof(CarId), nameof(RentalId))]
public class RentalItem {
        private Car car;
        private Rental rentals;

        public int CarId { get; set; }
    [Range(1,int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    public int RentalId { get; set; }
        public RentalItem() { }
        public RentalItem(int carId, int quantity, int rentalId)
        {
            CarId = carId;
            Quantity = quantity;
            RentalId = rentalId;
        }

        public RentalItem(Car car, Rental rentals)
        {
            this.car = car;
            this.rentals = rentals;
        }

        public RentalItem(int id, Rental rental, float rentingPrice, string manufacturer)
        {
            Id = id;
            Rental = rental;
            RentingPrice = rentingPrice;
            Manufacturer = manufacturer;
        }

        public RentalItem(int id, int quantity, Rental rental, float rentingPrice1, string manufacturer, double rentingPrice2)
        {
            Id = id;
            Quantity = quantity;
            Rental = rental;
            Manufacturer = manufacturer;
        }

        public Car Car { get; set; }

    public Rental Rental { get; set; }
        public int Id { get; }
        public float RentingPrice { get; }
        public string Manufacturer { get; }
    }
}