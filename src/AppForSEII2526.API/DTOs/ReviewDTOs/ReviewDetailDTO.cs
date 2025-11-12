





namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO : ReviewForCreateDTO
    {
        // Constructor compatible con la proyección usada en ReviewController:
        // new ReviewDetailDTO(p.Id, p.country, p.created, p.ApplicationUser.UserName, (DriverType)p.drivertype, reviewItemsList)
        public ReviewDetailDTO(int id, string country, DateTime created, string userName, DriverType drivertype, IList<ReviewItemDTO> reviewItems)
            : base(country, drivertype, userName, reviewItems, manufacturer: string.Empty, color: string.Empty, rating: 0, description: string.Empty, model: string.Empty, fueltype: string.Empty)
        {
            Id = id;
            Created = created;
            ReviewItems = reviewItems ?? new List<ReviewItemDTO>();
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }

        // Ahora exponemos ReviewItems como DTOs (coincide con lo que proyectas en el controlador)
        public IList<ReviewItemDTO> ReviewItems { get; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   base.Equals(dTO) &&
                   Id == dTO.Id &&
                   Created.Equals(dTO.Created);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Created);
        }
    }
}