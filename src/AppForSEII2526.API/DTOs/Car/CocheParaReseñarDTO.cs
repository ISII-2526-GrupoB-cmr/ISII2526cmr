using System;

namespace AppForSEII2526.API.DTOs.Car
{
    public class CocheParaReseñarDTO : IEquatable<CocheParaReseñarDTO>
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string FuelType { get; set; }
        public string Manufacturer { get; set; }

        public CocheParaReseñarDTO(int id, string model, string color, string fuelType, string manufacturer)
        {
            Id = id;
            Model = model;
            Color = color;
            FuelType = fuelType;
            Manufacturer = manufacturer;
        }

        // Implementación de igualdad para que Assert.Equal compare por valores.
        public bool Equals(CocheParaReseñarDTO? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id
                && string.Equals(Model, other.Model, StringComparison.Ordinal)
                && string.Equals(Color, other.Color, StringComparison.Ordinal)
                && string.Equals(FuelType, other.FuelType, StringComparison.Ordinal)
                && string.Equals(Manufacturer, other.Manufacturer, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj) => Equals(obj as CocheParaReseñarDTO);

        public override int GetHashCode()
        {
            return HashCode.Combine(Id,
                Model ?? string.Empty,
                Color ?? string.Empty,
                FuelType ?? string.Empty,
                Manufacturer ?? string.Empty);
        }
    }
}
