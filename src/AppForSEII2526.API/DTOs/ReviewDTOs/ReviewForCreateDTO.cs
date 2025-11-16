
using AppForSEII2526.API.Models;
using System.Net;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO
    {
        public ReviewForCreateDTO(string country, DriverType driverType, string username, IList<ReviewItemDTO> reviewitems, DateTime created) {

            Created = created;
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Drivertype = driverType;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Reviewitems= reviewitems ?? throw new ArgumentNullException(nameof(reviewitems));
        }
       
        

        [Display(Name = "Country")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
        public string Country { get; set; }

        [Required]
        public DriverType Drivertype { get; set; }

        [Display (Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your username is required")]
        public string Username { get; set; }

        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        public IList<ReviewItemDTO> Reviewitems { get; set; }

      

        public override bool Equals(object? obj)
        {
            return obj is ReviewForCreateDTO dTO &&
                   Country == dTO.Country &&
                   Username == dTO.Username &&
                   Drivertype == dTO.Drivertype &&
                   Created == dTO.Created &&
                    Reviewitems.SequenceEqual(dTO.Reviewitems);

        }
    }
}
