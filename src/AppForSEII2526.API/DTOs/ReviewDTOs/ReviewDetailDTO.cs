
using System;
using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO : ReviewForCreateDTO
    {
        public ReviewDetailDTO(int id, string country, DateTime created, string driverType, string userName, IList<ReviewItemDTO> reviewItems)
            
        {
            Id = id;
            Created = created;
            DriverType = driverType;
            Country = country;
            UserName= userName;
            ReviewItems= reviewItems;
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string DriverType { get; set; }
        public string Country { get; set; }
        public string UserName { get; set; }
        public IList<ReviewItemDTO> ReviewItems { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewDetailDTO dTO &&
                   base.Equals(obj) &&
                   Id == dtO.Id &&
                   Created.Equals(dTO.Created);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Id, Created);
        }
    }
}