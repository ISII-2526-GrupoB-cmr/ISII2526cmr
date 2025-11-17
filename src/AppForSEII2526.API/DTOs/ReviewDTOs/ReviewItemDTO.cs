
using System;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewItemDTO
    {

        public ReviewItemDTO(string model, string fueltype, string manufacturer, string color, float rating, string? description)

        {
           
            Model = model;
            Fueltype = fueltype;
            Manufacturer = manufacturer;
            Color = color;
            Rating = rating;
            Description = description;
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo model es obligatorio")]
        public string Model { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo fueltype es obligatorio")]
        public string Fueltype { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo manufacturer es obligatorio")]
        public string Manufacturer { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo color es obligatorio")]
        public string Color { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo rating es obligatorio")]
        [Range(1, 5, ErrorMessage = "La nota debe estar entre 1 y 5")]
        public float Rating { get; set; }
        public string? Description { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&
                   
                   Model == dTO.Model &&
                   Manufacturer == dTO.Manufacturer &&
                   Fueltype == dTO.Fueltype &&
                   Color == dTO.Color &&
                   Rating == dTO.Rating &&
                   Description == dTO.Description;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Model, Fueltype, Manufacturer, Color, Rating, Description);
        }
    }
}
