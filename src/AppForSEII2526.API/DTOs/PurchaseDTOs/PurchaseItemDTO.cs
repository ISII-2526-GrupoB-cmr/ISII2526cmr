using System.Drawing;

public class PurchaseItemDTO
{
    public PurchaseItemDTO ()
    {

    }

    /*
    5. El sistema muestra los coches que el cliente ha seleccionado,
    indicando su modelo, color, descripción y precio, 
    así como el precio total de la compra
    */

    public PurchaseItemDTO(int carID, float purchasePrice, string modelo, string carColor, int quantity,string description = "")
    {
        CarID = carID;
        CarColor = carColor;
        PurchasePrice = purchasePrice;
        Modelo = modelo;
        Quantity = quantity;
        Description = description;
    }

    public int Quantity { get; set; }
    public int CarID { get; set; }
    public string CarColor { get; set; }

    public double PurchasePrice { get; set; }

    public string? Description { get; set; }

    public string Modelo { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is PurchaseItemDTO dTO &&
               CarID == dTO.CarID &&
               CarColor == dTO.CarColor &&
               PurchasePrice == dTO.PurchasePrice &&
               Description == dTO.Description &&
               Modelo == dTO.Modelo;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(CarID, CarColor, PurchasePrice, Description, Modelo);
    }
}
