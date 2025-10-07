using Humanizer.Localisation;

public class Car
{
    [Display(Name = "Car class")]
    public string CarClass { get; set; }
    public string Color { get; set; }
    public string Description { get; set; }
    [Display(Name = "Engage displacement")]
    public string EngDisplacement { get; set; }
    [Display(Name = "Fuel type")]
    public string FuelType { get; set; }
    public int Id { get; set; }
    [Display(Name = "Maintenance type")]
    public string MaintenanceType { get; set; }
    public string Manufacturer { get; set; }
    [Display(Name = "Purchase item")]
    public int PurchaseItem { get; set; }
    [Display(Name = "Purchase price")]
    public float PurchasePrice { get; set; }

    [Display (Name ="Quantity for purchase")]
    public float QuantityForPurchase { get; set; }
    [Display(Name = "Quantity for renting")]
    public float QuantityForRenting { get; set; }

    [Display(Name = "Renting price")]
    public float RentingPrice { get; set; }
    [Display(Name = "Rental item")]
    public float RentalItem { get; set; }
    [Display(Name = "Rim size")]
    public float RimSize { get; set; }

    public IList<ReviewItem> ReviewItems { get; set; }
    public IList<RentalItem> RentalItems { get; set; }
    public IList<PurchaseItem> PurchaseItems { get; set; }

    public Model Model { get; set; }


}