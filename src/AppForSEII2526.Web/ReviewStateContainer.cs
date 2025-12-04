
using AppForSEII2526.Web.API;


namespace AppForSEII2526.Web
{
    public class ReviewStateContainer
    {

        //we create an instance of Rental when an instance of RentalStateContainer is created
        public ReviewForCreateDTO Review { get; private set; } = new ReviewForCreateDTO()
        {
            Reviewitems = new List<ReviewItemDTO>()
        };

        //we compute the TotalPrice of the movies we have selected for renting them
       

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();



        public void AddCarToReview(CocheParaReseñarDTO car)
        {
            //before adding a movie we checked whether it has been already added
            if (!Review.Reviewitems.Any(ri => ri.Model == car.Model))
                //we add it if it is not in the list
                Review.Reviewitems.Add(new ReviewItemDTO()
                {
                    Model = car.Model,
                    Manufacturer = car.Manufacturer,
                    Color= car.Color,
                    Fueltype= car.FuelType,
                   
                }
            );

        }

        //to delete movies from the list of selected movies
        public void RemoveReviewItemToReview(ReviewItemDTO item)
        {
            Review.Reviewitems.Remove(item);

        }

        //we eliminate all the movies from the list
        public void ClearReviewList()
        {
            Review.Reviewitems.Clear();

        }

        //we have already finished the process of renting, thus, we create a new Rental 
        public void ReviewProcessed()
        {
            //we have finished the rental process so we create a new object without data
            Review = new ReviewForCreateDTO()
            {
                Reviewitems = new List<ReviewItemDTO>()
            };
        }
    }
}
