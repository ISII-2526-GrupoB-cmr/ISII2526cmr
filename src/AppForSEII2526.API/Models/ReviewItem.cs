namespace AppForSEII2526.API.Models
{
    [PrimaryKey(nameof(CarId), nameof(ReviewId))]
    public class ReviewItem
    {
        public Car Car { get; set; }
        public int CarId { get; set; }
        public String? Description { get; set; }

        [Range(1,5, ErrorMessage = "The mark must be between 1 and 5")]
        public int Rating { get; set; }
        public int ReviewId { get; set; }
        public Review Review { get; set; } 
    }
}
