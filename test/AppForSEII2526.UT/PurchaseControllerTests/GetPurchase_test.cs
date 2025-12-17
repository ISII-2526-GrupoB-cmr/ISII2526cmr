using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseDTOs;

namespace AppForSEII2526.UT.PurchaseControllerTests
{
    public class GetPurchase_test : AppForSEII25264SqliteUT
    {
        public GetPurchase_test()
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

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            //Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;

            var controller = new PurchasesController(_context, logger);

            //Act
            var result = await controller.GetPurchase(0);

            //Assert
            //we check that the response type is OK and obtain the list of cars
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetPurchase_Found_test()
        {
            //Arrange
            var mock = new Mock<ILogger<PurchasesController>>();
            ILogger<PurchasesController> logger = mock.Object;
            var controller = new PurchasesController(_context, logger);

            var expectedPurchase1 = new PurchaseDetailDTO(
                1,
                DateTime.Now,
                "Elena",
                "Navarro Martínez",
                "Calle Falsa 1",
                230000,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(
                        1,
                        230000.0f,
                        "Modelo A",
                        "Red",
                        1,
                        null
                    )
                }
            );

            //Act
            var result = await controller.GetPurchase(1);

            //Assert
            //we check that the response type is OK and obtain the rental
            var okResult = Assert.IsType<OkObjectResult>(result);
            var purchaseDTOActual = Assert.IsType<PurchaseDetailDTO>(okResult.Value);
            var eq = expectedPurchase1.Equals(purchaseDTOActual);

            //we check that the expected and actual are the same
            Assert.Equal(expectedPurchase1, purchaseDTOActual);
        }
    }
}
