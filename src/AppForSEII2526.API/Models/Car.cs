public class Car
{
    public string CarClass { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    public string EngDisplacement { get; set; }
    public string FuelType { get; set; }
    public int Id { get; set; }
    public string MaintenanceType { get; set; }
    public string Manufacturer { get; set; }
    public int PurchaseItems { get; set; }
    public float PurchasePrice { get; set; }
    public float QuantityForPurchase { get; set; }
    public float QuantityForRenting { get; set; }

    public float RentingPrice { get; set; }
    public float ReviewItems { get; set; }
    public float RimSize { get; set; }
    public IList<RentalItem> RentalItems { get; set; }
}