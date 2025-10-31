using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewItemDTO
    {
        public ReviewItemDTO(int carid, string model, string fueltype, string manufacturer, string color)
        {
            Carid = carid;
            Model = model;
            Fueltype = fueltype;
            Manufacturer = manufacturer;
            Color = color;
        }

        public int Carid { get; set; }

        public string Model { get; set; }

        public string Fueltype { get; set; }

        public string Manufacturer { get; set; }

        public string Color { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ReviewItemDTO dTO &&
                   Carid == dTO.Carid &&
                   Model == dTO.Model &&
                   Manufacturer == dTO.Manufacturer &&
                   Fueltype == dTO.Fueltype &&
                   Color == dTO.Color;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Carid, Model, Fueltype, Manufacturer, Color);
        }
    }
}
