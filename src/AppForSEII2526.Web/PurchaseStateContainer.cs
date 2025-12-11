using AppForSEII2526.Web.API;



namespace AppForSEII2526.Web
{
    public class PurchaseStateContainer
    {

        //we create an instance of Rental when an instance of RentalStateContainer is created
        public PurchaseForCreateDTO Purchase { get; private set; } = new PurchaseForCreateDTO() {
            PurchaseItems = new List<PurchaseItemDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
        public decimal TotalPrice //???
        {
            get
            {
                return Convert.ToDecimal(Purchase.PurchaseItems.Sum(pi => pi.PurchasePrice));
            }
        }

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddCarToPurchase(CocheParaComprarDTO car)
        {
            //before adding a movie we checked whether it has been already added
            if (!Purchase.PurchaseItems.Any(pi => pi.CarID == car.Id))
                //we add it if it is not in the list
                Purchase.PurchaseItems.Add(new PurchaseItemDTO()
                {
                    CarID = car.Id,
                    Modelo = car.Model,
                    CarColor = car.Color,
                    PurchasePrice = car.PurchasePrice,
                    Description = car.
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemovePurchaseItemToPurchase(PurchaseItemDTO item)
        {
            Purchase.PurchaseItems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearPurchaseCart()
        {
            Purchase.PurchaseItems.Clear();
            
        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void PurchaseProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Purchase = new PurchaseForCreateDTO()
            {
                PurchaseItems = new List<PurchaseItemDTO>()
            };
        }
    }
}