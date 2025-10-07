namespace AppForSEII2526.API.Models { 
[PrimaryKey(nameof(CarId), nameof(RentalId))]
public class RentalItem {
    public int CarId { get; set; }

    public int Quantity { get; set; }

    public int RentalId { get; set; }

    public Car Car { get; set; }

    public Rental Rental { get; set; }

}
}