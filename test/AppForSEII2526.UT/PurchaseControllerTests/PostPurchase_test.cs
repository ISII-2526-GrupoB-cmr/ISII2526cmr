using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UT.PurchaseControllerTests
{
    public class PostPurchase_test : AppForSEII25264SqliteUT
    {
        private const ApplicationUser trackeduser = null;
        
        public PostPurchase_test()
        {
            var models = new List<Model>()
            {
                new Model("Modelo A"),
                new Model("Modelo B"),
                new Model("Modelo C"),
            };
            var cars = new List<Car>()
                {
                    new Car(models[0], "Deportivo", "Red", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 230000.0f, 3, 2, 1500.0f, 19.0f),
                    new Car(models[1], "Sedán", "Blue", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 3, 500.0f, 16.0f),
                    new Car(models[2], "SUV", "Black", "SUV espacioso", "2.5L", "Híbrido", 3, "Preventivo", "Honda", 1003, 40000.0f, 4, 2, 600.0f, 18.0f),
                };

            _context.AddRange(models);
            _context.AddRange(cars);
            _context.SaveChanges();

            ApplicationUser user = new ApplicationUser("1", "Elena", "Navarro Martínez", "elena@uclm.es");
            _context.ApplicationUsers.Add(user);
            _context.SaveChanges();

            // Usar la instancia rastreada por el DbContext (con Id asignado) antes de crear la compra
            var trackedUser = _context.ApplicationUsers
                .First(au => au.Name == user.Name && au.Surname == user.Surname);

            

            var purchase1 = new Purchase(trackedUser, "AutoGo", 0, DateTime.Now, cars[0].PurchasePrice, "Calle Falsa 1", new List<PurchaseItem>());
            purchase1.PurchaseItems.Add(new PurchaseItem(cars[0], purchase1, 1));
            _context.Add(purchase1);
            _context.SaveChanges();

        }

        public static IEnumerable<object[]> TestCasesFor_PostPurchase()
        {
            
            var purchaseNoItem = new PurchaseForCreateDTO("Elena", "Navarro Martínez", "Avda. España 2, Albacete", 0, new List<PurchaseItemDTO>());

                var purchaseItems = new List<PurchaseItemDTO>() { new PurchaseItemDTO(1, 230000, "Toyota Corolla", "Gris", 1, "Sedán cómodo y eficiente, ideal para ciudad.") };
                var purchaseItemsCant0 = new List<PurchaseItemDTO>() { new PurchaseItemDTO(1, 230000, "Toyota Corolla", "Gris", 0, "Sedán cómodo y eficiente, ideal para ciudad.") };

            var purchaseCantidadCero = new PurchaseForCreateDTO("Elena", "Navarro Martínez", "Avda. España 2, Albacete", 0, purchaseItemsCant0);

            var purchaseUserNoExist = new PurchaseForCreateDTO("Juan", "Pérez", "Calle Falsa 33, Chinchilla", 0, purchaseItems);

          

          

            var allTests = new List<object[]>
            {
                new object[] { purchaseNoItem, "Error! Ningun coche seleccionado" },
                new object[] { purchaseUserNoExist, "Error! Tu nombre no esta registrado" },
          
            };
            return allTests;
        }

        [Theory] //[Theory] significa que esta prueba se ejecuta una vez por cada caso de datos que devuelve TestCasesFor_PostPurchase
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_PostPurchase))]
        public async Task PostPurchase_Error_test(PurchaseForCreateDTO purchaseDTO, string expectedError)
        {
            //Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            //Act
            var result = await controller.CreatePurchase(purchaseDTO);

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
        public async Task PostPurchase_Success_test()
        {
            //Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            //la entrada que se le va a dar al metodo
            var purchaseDTO = new PurchaseForCreateDTO(
                "Elena",
                "Navarro Martínez",
                "Calle Falsa 1",
                PaymentMethodTypes.GooglePay,
                new List<PurchaseItemDTO>()
                {
                    new PurchaseItemDTO(1,2300000,"Modelo A","Red",1,"Deportivo rápido")
                }
            );

            //PurchaseDetailDTO que se espera que devuelva el metodo
            var expectedPurchase1 = new PurchaseDetailDTO(
                2,
                DateTime.Now,
                "Elena",
                "Navarro Martínez",
                "Calle Falsa 1",
                230000,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(
                        1,
                        2300000.0f,
                        "Modelo A",
                        "Red",
                        1,
                        "Deportivo rápido"
                    )
                }
            );
            //Act
            var result = await controller.CreatePurchase(purchaseDTO);


            //Assert
            //Se verifica que el resultado sea un CreatedAtActionResult (201)
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);

            // en actualPurchaseDetailDTO tenemos el DTO devuelto por el metodo
            var actualPurchaseDetailDTO = Assert.IsType<PurchaseDetailDTO>(createdResult.Value);

            //actualPurchaseDetailDTO.PurchaseDate= fixedDate; // Ajustar la fecha para la comparación

            

            Assert.Equal(expectedPurchase1, actualPurchaseDetailDTO);
        }
    }
}