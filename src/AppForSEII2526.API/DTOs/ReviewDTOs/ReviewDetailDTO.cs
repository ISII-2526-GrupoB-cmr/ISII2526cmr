
using System;
using System.Collections.Generic;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewDetailDTO : ReviewForCreateDTO
    {
        public ReviewDetailDTO(int id, string country, DateTime created, string driverType, string userName, IList<ReviewItemDTO> reviewItems, string manufacturer, string color, int rating, string description, string model)
            : base(
                    model,
                    manufacturer,
                    color,
                    rating,
                    description,
                    userName,
                    country,
                    driverType,
                    reviewItems
                 )
        {
            Id = id;
            Created = created;
        }

        public int Id { get; set; }
        public DateTime Created { get; set; }

        public override bool Equals(object? obj)
        {
            // Usar el objeto tipado para comparar la parte base correctamente
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