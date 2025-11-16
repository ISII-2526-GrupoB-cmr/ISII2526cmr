
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
        public int Carid { get; set; }
        public string Model { get; set; }
        public string Fueltype { get; set; }
        public string Manufacturer { get; set; }
        public string Color { get; set; }
        public float Rating { get; set; }
        public string? Description { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&
                   Carid == dTO.Carid &&
                   Model == dTO.Model &&
                   Manufacturer == dTO.Manufacturer &&
                   Fueltype == dTO.Fueltype &&
                   Color == dTO.Color &&
                   Rating == dTO.Rating &&
                   Description == dTO.Description;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Carid, Model, Fueltype, Manufacturer, Color, Rating, Description);
        }
    }
}
