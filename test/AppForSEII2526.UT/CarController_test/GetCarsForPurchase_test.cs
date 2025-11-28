using AppForSEII2526.API.DTOs.PurchaseDTOs;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.Car;

namespace AppForSEII2526.UT.CarController_test
{
    public class GetCarsForPurchase_test : AppForSEII25264SqliteUT
    {
        public GetCarsForPurchase_test()
        {
            var models = new List<Model>()
            {
                new Model("Ferrari 1"),
                new Model("Toyota 1"),
                new Model("Honda 1"),
            };
            var cars = new List<Car>()
                {
                    new Car(models[0], "Deportivo", "Azul", "Deportivo rápido", "2.0L", "Gasolina", 1, "Preventivo", "Ferrari", 1001, 230000.0f, 3, 2, 1500.0f, 19.0f),
                    new Car(models[1], "Sedán", "Azul", "Sedán cómodo", "1.6L", "Diésel", 2, "Correctivo", "Toyota", 1002, 30000.0f, 5, 3, 500.0f, 16.0f),
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

            var purchase1 = new Purchase(trackedUser, "AutoGo", 0, DateTime.Now, cars[0].PurchasePrice, new List<PurchaseItem>());
            purchase1.PurchaseItems.Add(new PurchaseItem(cars[0], purchase1));
            _context.Add(purchase1);
            _context.SaveChanges();
        }
        public static IEnumerable<object[]> TestCasesFor_GetCarsForPurchase_OK()
        {
            var cochesDTOs = new List<CocheParaComprarDTO>()
            {
                new CocheParaComprarDTO(1,"Ferrari 1","Azul","Gasolina","Ferrari",230000.0f),
                new CocheParaComprarDTO(2,"Toyota 1","Azul","Diésel","Toyota",30000.0f),
                new CocheParaComprarDTO(3,"Honda 1","Black","Híbrido","Honda",40000.0f),
            };

            var cochesDTOsTC1 = new List<CocheParaComprarDTO>() { cochesDTOs[0], cochesDTOs[1] };
            var cochesDTOsTC2 = new List<CocheParaComprarDTO>() { cochesDTOs[1] };
            var cochesDTOsTC3 = new List<CocheParaComprarDTO>() {  };
            var cochesDTOsTC4 = new List<CocheParaComprarDTO>() { cochesDTOs[0], cochesDTOs[1], cochesDTOs[2] };
            var cochesDTOsTC5 = new List<CocheParaComprarDTO>() { cochesDTOs[2] };

            var allTests = new List<object[]>
            {
                new object[] { cochesDTOsTC1, null , "Azul" },
                new object[] { cochesDTOsTC2, "Toyota 1" , null },
                new object[] { cochesDTOsTC3, "BMW 1" , null },
                new object[] { cochesDTOsTC4, null , null },
                new object[] { cochesDTOsTC5, "Honda 1" , "Black" },
            };
            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesFor_GetCarsForPurchase_OK))]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCarsForPurchase_OK_test(IList<CocheParaComprarDTO> expectedCars, string? modelo, string? color)
        {
            //Arrange
            var controller = new CarsController(_context, null);

            //Act
            var result = await controller.GetCocheParaComprar(color, modelo);

            //Assert
            //we check that the response type is OK
            var okResult = Assert.IsType<OkObjectResult>(result);
            //and obtain the list of cars
            var carDTOsActual = Assert.IsType<List<CocheParaComprarDTO>>(okResult.Value);

            Assert.Equal(expectedCars, carDTOsActual);
        }
    }
}
