using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_AlquilarCoche
{
    public class CUAlquilarCoche_UIT : UC_UIT
    {
        private SeleccionarCocheParaAlquilarPO seleccionarCocheParaAlquilarPO;
        private CreateCocheParaAlquilarPO createCocheParaAlquilarPO;
        private DetailCocheParaAlquilarPO detailCocheParaAlquilarPO;

        private const string Name = "Elena";
        private const string SurName = "Navarro Martínez";
        private const string DeliveryCarDealer = "Calle Albacete";
        private const string PaymentMethod = "Visa";
        private const string Quantity = "1";

        private const int carId = 1;
        private string ModelCar = "Toyota Corolla";
        private string ManufacturerCar = "Toyota";
        private string FuelType = "Gasoline";
        private string totalprice = "85";

        private const int carId2 = 2;
        private string ModelCar2 = "BMW M4";
        private string ManufacturerCar2 = "BMW";
        private string FuelType2 = "Gasoline";
        private string totalprice2 = "85";
        public CUAlquilarCoche_UIT(ITestOutputHelper output) : base(output)
        {
            seleccionarCocheParaAlquilarPO = new SeleccionarCocheParaAlquilarPO(_driver, output);
            createCocheParaAlquilarPO = new CreateCocheParaAlquilarPO(_driver, output);
            detailCocheParaAlquilarPO = new DetailCocheParaAlquilarPO(_driver, output);

        }



        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForRentalCars_UIT()
        {
            Precondition_perform_login();
            Thread.Sleep(500);


            seleccionarCocheParaAlquilarPO.WaitForBeingVisibleIgnoringExeptionTypes(By.Id("CreateRenting"));
            _driver.FindElement(By.Id("CreateRenting")).Click();


            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.Url.Contains("rental"));
            }
            catch (WebDriverTimeoutException)
            {
                _driver.Navigate().GoToUrl(new Uri(_driver.Url).GetLeftPart(UriPartial.Authority) + "/rental/select");
            }
        }

        [Theory]
        [InlineData(carId, Name, SurName, DeliveryCarDealer, PaymentMethod, Quantity)]

        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_1_2_3_BasicFlow(int carId, string name, string surname, string deliveryAddress, string paymentMethod, string quantity)
        {
            //Arrange

            var createrental = new CreateCocheParaAlquilarPO(_driver, _output);
            var detailRental = new DetailCocheParaAlquilarPO(_driver, _output);

            var startdate = DateTime.Today.AddDays(1);
            var enddate = DateTime.Today.AddDays(2);
            var rentingdate = DateTime.Today.AddDays(1);


            //Act
            InitialStepsForRentalCars_UIT();

            seleccionarCocheParaAlquilarPO.SearchCars(0, null);
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();

            createrental.fillInformationuser(name, surname, deliveryAddress, paymentMethod);
            createrental.FillInRentalQuantity(quantity, carId);
            createrental.PressRentCars();
            createrental.PressOkModalDialog();


            //Assert
            //the expected error is shown in the view
            Assert.True(detailRental.CheckDetailParaAlquilar(name, surname, deliveryAddress, paymentMethod, DateTime.Now, startdate, enddate, totalprice),
                "Error: detail rental is not as expected");



            Assert.True(detailRental.CheckDetailParaAlquilar(name, surname, deliveryAddress, paymentMethod, rentingdate, startdate, enddate, totalprice),
                "Error: rental items are not as expected");

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA0_CU2_2_CochesNoDisponibles()
        {

            InitialStepsForRentalCars_UIT();

            var expectedCars = "No cars available matching the criteria.";

            //Arrange
            seleccionarCocheParaAlquilarPO.SearchCars(0, "");

            //Act
            Assert.True(seleccionarCocheParaAlquilarPO.CheckMessageErrorNotAvailableCars(expectedCars));

        }


        [Theory]
        [InlineData("1", "Toyota Corolla", "Toyota", "Gasoline", "85")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA1_CU2_2_FiltradoPorPrecioYModelo(string carId, string model, string manufacturer, string fueltype, string totalprice)
        {
            InitialStepsForRentalCars_UIT();
            //Arrange
            var expectedCars = new List<string[]>
            {
                new string[] { carId, model, manufacturer, fueltype, totalprice },
            };
            //Act
            int totalpriceInt = int.Parse(totalprice);
            seleccionarCocheParaAlquilarPO.SearchCars(totalpriceInt, model);
            //Assert
            Assert.True(seleccionarCocheParaAlquilarPO.CheckListOfCars(expectedCars));




        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA2_CU2_2_NingunCocheSeleccionado()
        {   //Arrange
            InitialStepsForRentalCars_UIT();

            //act
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar);

            //Assert
            Assert.True(seleccionarCocheParaAlquilarPO.RentingNotAvailable());


        }

        [Fact]
        [Trait("LevelTesting","Funcional Testing")]
        public void UC2_FA3_CU2_2_ModificarCarroYactTotal()
        {
            InitialStepsForRentalCars_UIT();

            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();

            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId);
            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId2);
            createCocheParaAlquilarPO.PressModifyCars();

            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();
        }
        

        [Theory]
        [InlineData("", "Navarro Martínez", "Calle Albacete", "Visa")]
        [InlineData("Elena", "", "Calle Albacete", "Visa")]
        [InlineData("Elena", "Navarro Martínez", "", "Visa")]
        [Trait("LevelTesting","Funcional Testing")]
        public void UC2_FA4_CU2_2_ErrorDatosObligatorios(string name,string surname,string address,string paymentmethod)
        {
            InitialStepsForRentalCars_UIT();
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();
            createCocheParaAlquilarPO.fillInformationuser(name, surname, address, paymentmethod);
            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId);
            createCocheParaAlquilarPO.PressRentCars();


            Assert.True(createCocheParaAlquilarPO.CheckErrorMessageForMandatoryFields(),
                "Error: The error message for mandatory fields is not shown as expected.");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA5_CU2_2_ModificarCochesSeleccionados()
        {
            InitialStepsForRentalCars_UIT();
            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();
            createCocheParaAlquilarPO.PressModifyCars();
            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();

            var expected = new List<String[]> { new String[] { ModelCar, ManufacturerCar, totalprice } };

            //Esto es el assert
            Assert.True(createCocheParaAlquilarPO.CheckListOfRentalItems(expected));
        }



    }
}
