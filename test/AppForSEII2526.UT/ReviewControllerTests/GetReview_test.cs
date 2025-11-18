using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTOs;

namespace AppForSEII2526.UT.ReviewControllerTests
{
    public class GetReview_test : AppForSEII25264SqliteUT
    {
        public GetReview_test()
        {
            var models = new List<Model>()
            {
                new Model("Modelo A"),
                new Model("Modelo B"),
                new Model("Modelo C"),
            };
            var cars = new List<Car>()
            {
                new Car(models[0], "Deportivo", "Red", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 120000.0f, 3.0f, 2.0f, 1500.0f, 19.0f),
                new Car(models[1], "Sedán", "Blue", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5.0f, 3.0f, 500.0f, 16.0f),
                new Car(models[2], "SUV", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4.0f, 2.5f, 600.0f, 18.0f),
            };

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.SaveChanges();

            var user = new ApplicationUser("elena@uclm.es", "Elena", "Navarro Martínez", "Calle López 1");
            
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            // Usar la instancia rastreada por el DbContext (con Id asignado) antes de crear la compra
            var trackedUser = _context.ApplicationUsers
                .First(au => au.UserName== user.UserName);

            var fixedDate = new DateTime(2025, 11, 17);
            var review1 = new Review("España", fixedDate, trackedUser, 0, new List<ReviewItem>());
            var reviewItem1 = new ReviewItem(1, "Buen coche", 3, review1);

            review1.ReviewItems.Add(reviewItem1);

            
            
            _context.Add(review1);
            _context.AddRange(reviewItem1);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetReview_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            var controller = new ReviewController(_context, logger);

            //Act
            var result = await controller.GetReview(0);

            //Assert
            //we check that the response type is OK and obtain the list of cars
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetReview_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);
            var fixedDate = new DateTime(2025, 11, 17);
            var expectedReview1 = new ReviewDetailDTO(
                    1,
                    "España",
                    fixedDate,
                    "elena@uclm.es",
                    0,
                    new List<ReviewItemDTO>
                    {
                        new ReviewItemDTO(
                            
                            "Modelo A",
                            "Gasolina",
                            "Ferrari",
                            "Red",
                            3.0f,
                            "Buen coche"
                        )
                    }
            );

            //Act
            var result = await controller.GetReview(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reviewDTOActual = Assert.IsType<ReviewDetailDTO>(okResult.Value);
            var eq = expectedReview1.Equals(reviewDTOActual);

            //we check that the expected and actual are the same
            Assert.Equal(expectedReview1, reviewDTOActual);
        }
    }
}
