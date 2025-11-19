using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.RentalDTOs;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.RentalsController_test
{
    public class PostRentals_test : AppForSEII25264SqliteUT
    {

        public PostRentals_test()
        {

            var modelos = new List<Model>() {
                new Model("Ferrari F8"),
                new Model("Toyota Corolla"),
                new Model("Honda Civic")
            };

            var cars = new List<Car>()
                {
                    new Car(modelos[0], "Deportivo", "Red", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 230000.0f, 3, 2, 1500.0f, 19.0f),
                    new Car(modelos[1], "Sedán", "Blue", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 3, 500.0f, 16.0f),
                    new Car(modelos[2], "SUV", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4, 2, 600.0f, 18.0f),
                };
            _context.AddRange(modelos);
            _context.AddRange(cars);
            _context.SaveChanges();

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martinez", "elena@uclm.es");
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            var trackedUser = _context.ApplicationUsers.First(au => au.Name == user.Name && au.Surname == user.Surname);

            var fixedDate = new DateTime(2025, 11, 16, 21, 28, 52);

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

            var rentalItem = new RentalItem
            {
                Car = cars[1],
                CarId = cars[1].Id,
                Rental = rentals,
                Quantity = 1,
                RentingPrice = 85,
                Manufacturer = cars[1].Manufacturer // 👈 este campo es el que antes fallaba
            };
            rentals.RentalItems.Add(rentalItem);


            _context.Add(rentals);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase()
        {
            var rentalNoITem = new RentalForCreateDTO("Elena", "Navarro Martinez", 1,
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5), new List<RentalItemDTO>());

            var rentalItems = new List<RentalItemDTO>() { new RentalItemDTO(2, "Toyota Corolla", "Toyota", 85) };

            var rentalFromBeforeToday = new RentalForCreateDTO("Elena", "Navarro Martinez", 1,
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today, DateTime.Today.AddDays(5), rentalItems);

            var rentalToBeforeFrom = new RentalForCreateDTO("Elena", "Navarro Martinez", 1,
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(5), DateTime.Today.AddDays(2), rentalItems);

            var RentalApplicationUser = new RentalForCreateDTO("victor.lopez@uclm.es", "lopez muñoz", 1,
                "C/ Postigos 20", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(4), rentalItems);

           

            var CantidadMCero = new RentalForCreateDTO("Elena", "Navarro Martinez", 0,
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(2), DateTime.Today.AddDays(5),
                new List<RentalItemDTO>() { new RentalItemDTO(2, "Toyota Corolla", "Toyota", 85) });


            var allTests = new List<object[]>
            {             
                new object[] { rentalNoITem, "Error! Ningun vehiculo seleccionado",  },
                new object[] { rentalFromBeforeToday, "Error! La fecha de alquiler tiene que ser posterior a la de hoy", },
                new object[] { rentalToBeforeFrom, "Error! La fecha de fin tiene que ser mas tarde de la que empieza", },
                new object[] { RentalApplicationUser, "Error! El usuario no esta registrado", },
                new object[] { CantidadMCero, "Error! La cantidad para alquiilar tiene que ser mayor a 0", },

            };

            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreatePurchase))]
        public async Task CreateRental_Error_test(RentalForCreateDTO rentalDTO, string errorExpected)
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = problemDetails.Errors.First().Value[0];

            //we check that the expected error message and actual are the same
            Assert.StartsWith(errorExpected, errorActual);

        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task CreateRental_Success_test()
        {
            // Arrange
            var mock = new Mock<ILogger<RentalsController>>();
            ILogger<RentalsController> logger = mock.Object;

            var controller = new RentalsController(_context, logger);



            var rentalDTO = new RentalForCreateDTO("Elena", "Navarro Martinez", 1,
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(6), DateTime.Today.AddDays(7), new List<RentalItemDTO>()
                { new RentalItemDTO(2,"Toyota Corolla", "Toyota", 85) });

            var fixedDate = new DateTime(2025, 11, 16, 21, 28, 52);


            var expectedrentalDetailDTO = new RentalDetailDTO(2,fixedDate ,
                "Elena", "Navarro Martinez",
              "Avda. España 2, Albacete", PaymentMethodTypes.Visa,
                DateTime.Today.AddDays(6), DateTime.Today.AddDays(7), new List<RentalItemDTO>()
                { new RentalItemDTO(2, "Toyota Corolla", "Toyota", 85) });

            // Act
            var result = await controller.CreateRental(rentalDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var actualRentalDetailDTO = Assert.IsType<RentalDetailDTO>(createdResult.Value);

            Assert.Equal(expectedrentalDetailDTO, actualRentalDetailDTO);

        }

    }
}
