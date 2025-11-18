
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO
    {
        public ReviewDetailDTO(int id, string country, DateTime created, string userName, DriverType drivertype, IList<ReviewItemDTO> reviewItems)
        {
            Id = id;
            Country = country;
            Created = created;
            UserName = userName;
            Drivertype = drivertype;
            ReviewItems = reviewItems;
        }

        public int Id { get; set; }
        public string Country { get; set; }

        public string UserName { get; set; }

        public DriverType Drivertype { get; set; }

        
        public DateTime Created { get; set; }


        public IList<ReviewItemDTO> ReviewItems { get; set; }








        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                  
                   Country == dTO.Country &&
                   UserName == dTO.UserName &&
                   Drivertype == dTO.Drivertype &&
                   ReviewItems.SequenceEqual(dTO.ReviewItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Created);
        }
    }
}