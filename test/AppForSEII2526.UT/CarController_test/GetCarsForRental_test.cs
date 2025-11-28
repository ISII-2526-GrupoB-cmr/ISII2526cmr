
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.Car;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;
namespace AppForSEII2526.UT.CarController_test
{
    public class GetCarsForRental_test : AppForSEII25264SqliteUT
    {
        public GetCarsForRental_test()
        {
            var modelos = new List<Model>() {
            new Model("Ferrari 1"),
            new Model("Toyota 1"),
            new Model("Honda 1")
        };

            var cars = new List<Car>() {
                new Car(modelos[0], "Ferrari 1", "Azul", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 230000.0f, 3, 2, 1500.0f, 19.0f),
                new Car(modelos[1], "Toyota 1", "Azul", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 1, 500.0f, 16.0f),
                new Car(modelos[2], "Honda 1", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4, 2, 600.0f, 18.0f)
            };
            _context.AddRange(modelos);
            _context.AddRange(cars);
            _context.SaveChanges();


            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");

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
                Car = cars[0],
                CarId = cars[0].Id,
                Rental = rentals,
                Quantity = 1,
                RentingPrice = 85,
                Manufacturer = cars[0].Manufacturer // 👈 este campo es el que antes fallaba
            };

            rentals.RentalItems.Add(rentalItem);

            _context.Rentals.Add(rentals);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetMoviesForRental_OK()
        {

            var cochesDTOs = new List<CocheParaAlquilarDTO>()
            {
                new CocheParaAlquilarDTO(1, "Ferrari 1", "Azul", "Gasolina", "Ferrari", 1500.0f),
                new CocheParaAlquilarDTO(2, "Toyota 1", "Azul", "Diésel", "Toyota", 500.0f),
                new CocheParaAlquilarDTO(3, "Honda 1", "Black", "Híbrido", "Honda", 600.0f)
            };


            var cochesDTOsTC1 = new List<CocheParaAlquilarDTO>() { cochesDTOs[0] }; // Ferrari 1
            var cochesDTOsTC2 = new List<CocheParaAlquilarDTO>() { cochesDTOs[1] }; // Toyota 1
            var cochesDTOsTC3 = new List<CocheParaAlquilarDTO>() { cochesDTOs[0], cochesDTOs[1], cochesDTOs[2] }; // precio = 0 → todos
            var cochesDTOsTC4 = new List<CocheParaAlquilarDTO>() { cochesDTOs[2] }; // Honda 1


            var allTests = new List<object[]>
            {
                new object[] { 1500.0f, "Ferrari 1", cochesDTOsTC1 },
                new object[] { 500.0f, "Toyota 1", cochesDTOsTC2 },
                new object[] { 0f, null, cochesDTOsTC3 },
                new object[] { 600.0f, "Honda 1", cochesDTOsTC4 },

            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMoviesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCoche_Para_Alquilar_OK_test(float precio, string? model, List<CocheParaAlquilarDTO> expectedCars)
        {
            // Arrange
            var controller = new CarsController(_context, null);

            // Act
            var result = await controller.GetCocheParaAlquilar(precio, model);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var CarDTOsActual = Assert.IsType<List<CocheParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedCars, CarDTOsActual);

        }

    }
}
