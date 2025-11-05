using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using System.Net;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreateDTO
    {
        public ReviewForCreateDTO(string country, DriverType driverType, string username, IList<ReviewItemDTO> reviewitems, string manufacturer, string color, int rating, string description, string model) {


            Country = country ?? throw new ArgumentNullException(nameof(country));
            Drivertype = driverType;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Reviewitems= reviewitems ?? throw new ArgumentNullException(nameof(reviewitems));
            Manufacturer = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer)); 
            Color= color ?? throw new ArgumentNullException(nameof(color));
            Rating = rating;
            Description = description;
            Model= model;
        }
        public ReviewForCreateDTO(int rating, string model, string manufacturer)
        {
            Reviewitems = new List<ReviewItemDTO>();
          
        }

       

        [Display(Name = "Country")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
        public string Country { get; set; }

        [Required]
        public DriverType Drivertype { get; set; }

        [Display (Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Your username is required")]
        public string Username { get; set; }

        public IList<ReviewItemDTO> Reviewitems { get; set; }

        [Display(Name = "Manufacturer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
        public string Manufacturer { get; set; }

        [Display(Name = "Color")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
        public string Color { get; set; }

        [Required]
        public int Rating { get; set; }

        public string Description { get; set; }

        [Display(Name = "Model")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field cannot be empty")]
        public string Model { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewForCreateDTO dTO &&
                   Country == dTO.Country &&
                   Username == dTO.Username &&
                   Manufacturer == dTO.Manufacturer &&
                     Color == dTO.Color &&
                     Rating == dTO.Rating &&
                    Description == dTO.Description &&
                    Model == dTO.Model;
        }
    }
}
