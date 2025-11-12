using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RentalsController_test
{
    public class GetRentals_test : AppForSEII25264SqliteUT
    {
        public GetRentals_test()
        {
            var modelos = new List<Model>() {
                new Model("Sedan"),
                new Model("Model B"),
                new Model("Model C"),

            };

            var cars = new List<Car>(){
                new Car(modelos[0],"Sedan", "Red", "A red sedan", "2.0L", "Gasoline", 1, "Regular", "Toyota", 10, 20000, 5, 3, 85, 16),
                new Car(modelos[1],"SUV", "Blue", "A blue SUV", "3.0L", "Diesel", 2, "Premium", "Ford", 5, 30000, 2, 1, 150, 18),
                new Car(modelos[2],"Coupe", "Black", "A black coupe", "2.5L", "Gasoline", 3, "Regular", "Honda", 8, 25000, 4, 2, 120, 17)

            };

            _context.AddRange(modelos);
            _context.AddRange(cars);
            _context.SaveChanges();

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");


            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();



            var trackedUser = _context.ApplicationUsers.First(au=>au.Name == user.Name && au.Surname==user.Surname);

            var fixedDate= new DateTime(2025, 11, 16,21,28,52);


            var rentals = new Rental(
                trackedUser,
                "Central Dealer",
                PaymentMethodTypes.Visa,
                fixedDate,
                cars[0].RentingPrice,
                DateTime.Today.AddDays(2),
                DateTime.Today.AddDays(5),
                new List<RentalItem>()
        );

            // ✅ Crear RentalItem pasando todos los datos requeridos
            var rentalItem = new RentalItem
            {
                Car = cars[0],
                CarId = cars[0].Id,
                Rental = rentals,
                Quantity = 1,
                RentingPrice = 85,
                Manufacturer = cars[0].Manufacturer // 👈 este campo es el que antes fallaba
            };

            rentals.RentalItems.Add(rentalItem);

            _context.Add(rentals);
            _context.SaveChanges();
            

          
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetRental_NotFound_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.GetRental(0);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetRental_Found_test()
        {
            // Arrange
            var fixedDate = new DateTime(2025, 11, 16, 21, 28, 52);

            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;
            var controller = new RentalsController(_context, logger);


            var startDate = DateTime.SpecifyKind(DateTime.Today.AddDays(2), DateTimeKind.Local);
            var endDate = DateTime.SpecifyKind(DateTime.Today.AddDays(5), DateTimeKind.Local);

            var expectedRental = new RentalDetailDTO(1, fixedDate, "Elena", "Navarro Martínez",
                        "elena@uclm.es", PaymentMethodTypes.Visa,
                        startDate, endDate,
                        new List<RentalItemDTO> { new RentalItemDTO(1, "Sedan", "Toyota", 85) });


            // Act 
            var result = await controller.GetRental(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var rentalDTOActual = Assert.IsType<RentalDetailDTO>(okResult.Value);
            var eq = expectedRental.Equals(rentalDTOActual);
            //we check that the expected and actual are the same
            Assert.Equal(expectedRental, rentalDTOActual);

        }
    }
}