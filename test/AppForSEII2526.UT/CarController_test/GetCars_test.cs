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
    public class GetCars_test: AppForSEII25264SqliteUT
    {
        public GetCars_test() {
            var models = new List<Model>()
            {
                new Model("Model A"),
                new Model("Model B"),
                new Model("Model C")
            };
            var cars = new List<Car>()
            {
                new Car("Sedan", "Red", "A red sedan", "2.0L", "Gasoline", 1, "Regular", "Toyota", 10, 20000, 5, 3, 100, 16),
                new Car("SUV", "Blue", "A blue SUV", "3.0L", "Diesel", 2, "Premium", "Ford", 5, 30000, 2, 1, 150, 18),
                new Car("Coupe", "Black", "A black coupe", "2.5L", "Gasoline", 3, "Regular", "Honda", 8, 25000, 4, 2, 120, 17)
            };
            ApplicationUser user = new ApplicationUser("robermg", "Rober", "Moreno Gomez", "postigos");

            var rentals=new Rental("Central Dealer", PaymentMethodTypes.Visa, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), 500);

            rentals.RentalItems.Add(new RentalItem(cars[0], rentals));

            _context.ApplicationUsers.Add(user);
            _context.Models.AddRange(models);
            _context.Cars.AddRange(cars);
            _context.Rentals.Add(rentals);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetMoviesForRental_OK()
        {

            var cochesDTOs = new List<CocheParaAlquilarDTO>() {
                new CocheParaAlquilarDTO(1,"Sedan","rojo","gasolina","Toyota",85),
                new CocheParaAlquilarDTO(1,"Ibiza","negro","Diesel","Seat",1000),
                new CocheParaAlquilarDTO(1,"Toleda","blanco","gasolina","Seat",150),
            };

            var cochesDTOsTC1 = new List<CocheParaAlquilarDTO>() { cochesDTOs[1], cochesDTOs[2] }
                    //the GetMoviesForPurchase method returns the movies ordered by title
                    .OrderBy(m => m.Model).ToList();


            var cochesDTOsTC2 = new List<CocheParaAlquilarDTO>() { cochesDTOs[1] };
            var movieDTOsTC3 = new List<CocheParaAlquilarDTO>() { cochesDTOs[2] };

            var cochesDTOsTC3 = new List<CocheParaAlquilarDTO>() { cochesDTOs[0], cochesDTOs[1], cochesDTOs[2] }
                //the GetMoviesForPurchase method returns the movies ordered by title
                .OrderBy(m => m.Model).ToList();

            var allTests = new List<object[]>
            {             //filters to apply - expected movies
                                          //by default datefrom=today +1, dateto=today+2, thus movieDTOs[0] cannot be returned
                new object[] { null, null, null, null, cochesDTOsTC1,  },
                new object[] { "mechanic", null, null, null, cochesDTOsTC2, },
                new object[] { null, "Drama", null, null, cochesDTOsTC3, },
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetMoviesForRental_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCoche_Para_Alquilar_OK_test(float precio,string model,List<CocheParaAlquilarDTO> expectedCars)
        {
            // Arrange
            var controller = new CarsController(_context, null);

            // Act
            var result = await controller.GetCoche_Para_AlquilarDTO(precio, model);

            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of movies
            var CarDTOsActual = Assert.IsType<List<CocheParaAlquilarDTO>>(okResult.Value);
            Assert.Equal(expectedCars, CarDTOsActual);

        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetMoviesForRental_badrequest_test()
        {
            // Arrange
            var mock = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mock.Object;
            var controller = new CarsController(_context, logger);

            // Act
            var result = await controller.GetCoche_Para_AlquilarDTO(float precio,string modelo);

            //Assert
            //we check that the response type is OK and obtain the list of movies
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var problemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            var problem = problemDetails.Errors.First().Value[0];

            Assert.Equal("fromDate must be earlier than toDate", problem);
        }

    }
}
        
    

