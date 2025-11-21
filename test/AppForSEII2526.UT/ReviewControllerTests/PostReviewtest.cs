using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.ReviewControllerTests
{
    public class PostReview_test : AppForSEII25264SqliteUT
    {
        private const ApplicationUser trackeduser = null;
        private readonly DateTime fixedDate = new DateTime(2025, 11, 6, 21, 28, 52);
        public PostReview_test()
        {
            var models = new List<Model>()
            {
                new Model("Modelo A"),
                new Model("Modelo B"),
                new Model("Modelo C"),
            };
            var cars = new List<Car>()
            {
                new Car(models[0], "Deportivo", "Red", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 120000.0f, 3, 2, 1500.0f, 19.0f),
                new Car(models[1], "Sedán", "Blue", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 3, 500.0f, 16.0f),
                new Car(models[2], "SUV", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4, 2, 600.0f, 18.0f),
            };

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.SaveChanges();

            var user = new ApplicationUser("elena@uclm.es", "Elena", "Navarro Martínez", "Calle López 1");
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            // Usar la instancia rastreada por el DbContext (con Id asignado) antes de crear la compra
            var trackedUser = _context.ApplicationUsers
                .First(au => au.UserName == user.UserName);


            var review1 = new Review("España", DateTime.Today, trackedUser, 0, new List<ReviewItem>());
            var reviewItem1 = new ReviewItem(1, "Buen coche", 3, review1);

            review1.ReviewItems.Add(reviewItem1);


            _context.Add(review1);
            _context.AddRange(reviewItem1);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_PostReview()
        {
            var reviewNoCountry = new ReviewForCreateDTO("", 0, "elena@uclm.es", new List<ReviewItemDTO>());

            var reviewMalDriverType = new ReviewForCreateDTO("España", (DriverType)55, "elena@uclm.es", new List<ReviewItemDTO>());

            var reviewNoItem = new ReviewForCreateDTO("España", DriverType.experto, "elena@uclm.es", new List<ReviewItemDTO>());

            var reviewItems = new List<ReviewItemDTO>() { new ReviewItemDTO("Modelo B", "Diésel", "Toyota", "Blue", 3.0f, null) };

            var reviewCocheNoExiste = new ReviewForCreateDTO("España", DriverType.novato, "elena@uclm.es", new List<ReviewItemDTO>() { new ReviewItemDTO("Opel Corsa", "Opel", "Gasolina", "Red", 4.0f, null) });

            var reviewUserNoExiste = new ReviewForCreateDTO("España", DriverType.experto, "juanpepito@uclm.es", reviewItems);

            var reviewDescripcionErronea = new ReviewForCreateDTO("España", DriverType.experto, "elena@uclm.es", new List<ReviewItemDTO>() { new ReviewItemDTO("Modelo B", "Diésel", "Toyota", "Blue", 3.0f, "Mal coche") });


            var allTests = new List<object[]>
            {
                new object[] { reviewNoCountry, "Error! Pais de residencia no puede estar vacio" },
                new object[] { reviewMalDriverType, "Error! DriverType debe ser 'novato' o 'experto'." },
                new object[] { reviewNoItem, "Error! Ningun coche seleccionado para review" },
                new object[] { reviewCocheNoExiste, "Error! El coche seleccionado no existe" },
                new object[] { reviewUserNoExiste, "Error! Tu nombre de usuario no esta registrado" },
                new object[] { reviewDescripcionErronea, "Error! La reseña debe empezar por Reseña para"}

            };
            return allTests;
        }

        [Theory] //[Theory] significa que esta prueba se ejecuta una vez por cada caso de datos que devuelve TestCasesFor_PostPurchase
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_PostReview))]
        public async Task PostReview_Error_test(ReviewForCreateDTO reviewDTO, string expectedError)
        {
            //Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;

            var controller = new ReviewController(_context, logger);

            //Act
            var result = await controller.CreateReview(reviewDTO);

            //Assert
            //we check that the response type is BadRequest and obtain the error returned
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var ProblemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);

            var errorActual = ProblemDetails.Errors.First().Value[0]; //error que ha devuelto el metodo del controlador

            //we check that the expected error message and actual are the same
            Assert.StartsWith(expectedError, errorActual);
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task PostReview_Success_test()
        {
            //Arrange
            var mock = new Mock<ILogger<ReviewController>>();
            ILogger<ReviewController> logger = mock.Object;
            var controller = new ReviewController(_context, logger);

            //la entrada que se le va a dar al metodo
            var reviewDTO = new ReviewForCreateDTO(
                "España",
                0,
                "elena@uclm.es",
                new List<ReviewItemDTO>()
                {
                    new ReviewItemDTO("Modelo A", "Gasolina", "Ferrari", "Red", 3.0f, "Reseña para coche malo")
                }
            );


            var expectedReview1 = new ReviewDetailDTO(
                1,
                "España",
                DateTime.Today,
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
                       "Reseña para coche malo"
                    )
                }
            );
            //Act
            var result = await controller.CreateReview(reviewDTO);


            //Assert
            //Se verifica que el resultado sea un CreatedAtActionResult (201)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            // en actualPurchaseDetailDTO tenemos el DTO devuelto por el metodo
            var actualReviewDetailDTO = Assert.IsType<ReviewDetailDTO>(createdResult.Value);

            //actualPurchaseDetailDTO.PurchaseDate= fixedDate; // Ajustar la fecha para la comparación

            Assert.Equal(expectedReview1, actualReviewDetailDTO);
        }
    }
}