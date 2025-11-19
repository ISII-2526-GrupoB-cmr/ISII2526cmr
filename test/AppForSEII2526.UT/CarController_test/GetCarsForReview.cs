using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.Car;
using AppForSEII2526.API.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.CarsController_test
{
    public class GetCars_ForReview_test : AppForSEII25264SqliteUT
    {
        public GetCars_ForReview_test()
        {
            var modelos = new List<Model>() //creo una minibase de datos
            {
               
            new Model("Ferrari 1"),
            new Model("Toyota 1"),
            new Model("Honda 1")
            };

            var car = new List<Car>()
            {
               new Car(modelos[0], "Ferrari 1", "Azul", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 230000.0f, 3, 2, 1500.0f, 19.0f),
                new Car(modelos[1], "Toyota 1", "Azul", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 1, 500.0f, 16.0f),
                new Car(modelos[2], "Honda 1", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4, 2, 600.0f, 18.0f)
            };
            _context.AddRange(modelos);
            _context.AddRange(car);
            _context.SaveChanges();

            var user = new ApplicationUser("elena@uclm.es", "Elena", "Navarro Martínez", "Calle López 1");

            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            // Usar la instancia rastreada por el DbContext (con Id asignado) antes de crear la compra
            var trackedUser = _context.ApplicationUsers
                .First(au => au.UserName == user.UserName);

            var fixedDate = new DateTime(2025, 11, 17);
            var review1 = new Review("España", fixedDate, trackedUser, 0, new List<ReviewItem>());
            var reviewItem1 = new ReviewItem(1, "Buen coche", 3, review1);

            review1.ReviewItems.Add(reviewItem1);



            _context.Add(review1);
            _context.AddRange(reviewItem1);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetCarsForReview_OK()
        {
            var carDTOs = new List<CocheParaReseñarDTO>() 
            {
               new CocheParaReseñarDTO(1,"Ferrari 1","Azul","Gasolina","Ferrari"),
                new CocheParaReseñarDTO(2,"Toyota 1","Azul","Diésel","Toyota"),
                new CocheParaReseñarDTO(3,"Honda 1","Black","Híbrido","Honda"),

            };
            var carDTOsTC1 = new List<CocheParaReseñarDTO>() { carDTOs[0], carDTOs[1], carDTOs[2] };
            var carDTOsTC2 = new List<CocheParaReseñarDTO>() { carDTOs[0] };
            var carDTOsTC3 = new List<CocheParaReseñarDTO>() { carDTOs[2] };
            var carDTOsTC4 = new List<CocheParaReseñarDTO>() { carDTOs[1] };

            var allTest = new List<object[]> 
            {
                new object[] { null, null, carDTOsTC1 },
                new object[] { "Ferrari", null, carDTOsTC2 },
                new object[] { null, "Híbrido", carDTOsTC3 },
                new object[] { "Toyota", "Diésel", carDTOsTC4 },

            };

            return allTest;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForReview_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForReview_OK_test(string? filtroManufacturer, string? filtroFuelType, IList<CocheParaReseñarDTO> expectedCars)
        {
            // Arrange
            var mock = new Mock<ILogger<CarsController>>();
            ILogger<CarsController> logger = mock.Object;
            var controller = new CarsController(_context, logger);
            // Act
            var result = await controller.GetCoche_Para_Reseñar(filtroManufacturer, filtroFuelType); 
            //Assert
            //we check that the response type is OK 
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CocheParaReseñarDTO>>(okResult.Value);
            Assert.Equal(expectedCars, carDTOsActual); 
        }


    }
}