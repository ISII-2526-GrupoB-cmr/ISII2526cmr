using Humanizer.Localisation;
namespace AppForSEII2526.API.Models;

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
    [Display(Name = "Rim size")]
    public float RimSize { get; set; }

    public IList<ReviewItem> ReviewItems { get; set; }
    public IList<RentalItem> RentalItems { get; set; }
    public IList<PurchaseItem> PurchaseItems { get; set; }

    public Model Model { get; set; }
    public Car(string carClass, string color, string description, string engDisplacement, string fuelType, int id, string maintenanceType, string manufacturer, int purchaseItem, float purchasePrice, float quantityForPurchase, float quantityForRenting, float rentingPrice, float rimSize)
    {
        CarClass = carClass;
        Color = color;
        Description = description;
        EngDisplacement = engDisplacement;
        FuelType = fuelType;
        Id = id;
        MaintenanceType = maintenanceType;
        Manufacturer = manufacturer;
        PurchaseItem = purchaseItem;
        PurchasePrice = purchasePrice;
        QuantityForPurchase = quantityForPurchase;
        QuantityForRenting = quantityForRenting;
        RentingPrice = rentingPrice;
        RimSize = rimSize;
        
    }
}