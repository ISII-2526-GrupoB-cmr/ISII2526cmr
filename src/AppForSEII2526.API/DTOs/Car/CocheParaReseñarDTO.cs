namespace AppForSEII2526.API.DTOs.Car
{
    public class CocheParaReseñarDTO
    {

        public string carClass { get; set; }
        public int Id { get; set; }

        public string Model { get; set; }

        public string Color { get; set; }

        public string FuelType { get; set; }

        public string Manufacturer { get; set; }

       

        public CocheParaReseñarDTO(int id, string model, string color, string fuelType, string manufacturer, string CarClass)
        {
            Id = id;
            Model = model;
            Color = color;
            FuelType = fuelType;
            Manufacturer = manufacturer;
            carClass = CarClass;
           
        }

    }
}
