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
        private string totalprice2 = "350   ";
        private const int carId3 = 3;
        private string ModelCar3 = "Mazda CX-5";
        private string ManufacturerCar3 = "Mazda";
        private string FuelType3 = "Diesel";
        private string totalprice3 = "120";
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
            _output.WriteLine("Before login");
            Precondition_perform_login();

            _output.WriteLine("After login URL: " + _driver.Url);
            _output.WriteLine("Handles: " + _driver.WindowHandles.Count);

            seleccionarCocheParaAlquilarPO.WaitForBeingVisible(By.Id("CreateRenting"));
            _output.WriteLine("CreateRenting visible");
            _driver.FindElement(By.Id("CreateRenting")).Click();
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
            Thread.Sleep(1000);

            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();
            Thread.Sleep(1000);

            createrental.fillInformationuser(name, surname, deliveryAddress, paymentMethod);
            createrental.FillInRentalQuantity(quantity, carId);
            createrental.PressRentCars();
            createrental.PressOkModalDialog();
            Thread.Sleep(1000);


            //Assert
            //the expected error is shown in the view
            Assert.True(detailRental.CheckDetailParaAlquilar(name, surname, deliveryAddress, paymentMethod, DateTime.Now, startdate, enddate, totalprice),
                "Error: detail rental is not as expected");

            Thread.Sleep(1000);


            Assert.True(detailRental.CheckDetailParaAlquilar(name, surname, deliveryAddress, paymentMethod, rentingdate, startdate, enddate, totalprice),
                "Error: rental items are not as expected");

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA0_CU2_2_CochesNoDisponibles()
        {

            InitialStepsForRentalCars_UIT();

            var expectedCars = "No hay coches disponibles";
            Thread.Sleep(1000);

            //Arrange
            seleccionarCocheParaAlquilarPO.SearchCars(1, "");
            Thread.Sleep(1000);

            //Act
            Assert.True(seleccionarCocheParaAlquilarPO.CheckMessageErrorNotAvailableCars(expectedCars));

        }


        [Theory]
        [InlineData("1", "Toyota Corolla", "Toyota", "Gasoline", "85")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA1_CU2_2_FiltradoPorPrecioYModelo(string carId, string model, string manufacturer, string fueltype, string totalprice)
        {
            InitialStepsForRentalCars_UIT();
            Thread.Sleep(1000);

            //Arrange
            var expectedCars = new List<string[]>
            {
                new string[] {  model,fueltype, manufacturer,  totalprice },
            };
            //Act
            int totalpriceInt = int.Parse(totalprice);
            seleccionarCocheParaAlquilarPO.SearchCars(totalpriceInt, model);
            Thread.Sleep(1000);

            //Assert
            Assert.True(seleccionarCocheParaAlquilarPO.CheckListOfCars(expectedCars));




        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA2_CU2_2_NingunCocheSeleccionado()
        {   //Arrange
            InitialStepsForRentalCars_UIT();
            Thread.Sleep(1000);

            //act
            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar);
            Thread.Sleep(1000);

            //Assert
            Assert.True(seleccionarCocheParaAlquilarPO.RentingNotAvailable());


        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA3_CU2_2_ModificarCarroYactTotal()
        {
            InitialStepsForRentalCars_UIT();
            Thread.Sleep(1000);

            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();
            Thread.Sleep(1000);

            createCocheParaAlquilarPO.PressModifyCars();
            Thread.Sleep(1000);

            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();
        }


        [Theory]
        [InlineData("", "Navarro Martínez", "Calle Albacete", "Visa")]
        [InlineData("Elena", "", "Calle Albacete", "Visa")]
        [InlineData("Elena", "Navarro Martínez", "", "Visa")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA4_CU2_2_ErrorDatosObligatorios(
            string name,
            string surname,
            string address,
            string paymentmethod)
        {
            InitialStepsForRentalCars_UIT();

            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();

            createCocheParaAlquilarPO.fillInformationuser(name, surname, address, paymentmethod);
            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId);


            createCocheParaAlquilarPO.PressRentCars();
            Thread.Sleep(1000);
            Assert.True(
                createCocheParaAlquilarPO.CheckErrorMessageForMandatoryFields(),
                "Expected frontend validation error was not shown"
            );
        }


        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA5_CU2_2_ModificarCochesSeleccionados()
        {
            InitialStepsForRentalCars_UIT();
            Thread.Sleep(1000);

            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();
            Thread.Sleep(1000);
            createCocheParaAlquilarPO.PressModifyCars();
            Thread.Sleep(1000);

            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar2);
            seleccionarCocheParaAlquilarPO.RentCars();

            var expected = new List<String[]> { new String[] { ModelCar, totalprice, ManufacturerCar } };

            //Esto es el assert
            Assert.True(createCocheParaAlquilarPO.CheckListOfRentalItems(expected));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_FA4_CU2_2_ErroEnlasFechasDeAlquiler()
        {
            InitialStepsForRentalCars_UIT();

            seleccionarCocheParaAlquilarPO.SearchCars(0, "");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();
            createCocheParaAlquilarPO.fillInformationuser(Name, SurName, DeliveryCarDealer, PaymentMethod);
            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId);
            createCocheParaAlquilarPO.SetRentalDates(DateTime.Today.AddDays(3), DateTime.Today.AddDays(1));
            createCocheParaAlquilarPO.PressRentCars();
            Thread.Sleep(1000);
            Assert.True(
                createCocheParaAlquilarPO.CheckErrorMessageForInvalidDates(),
                "Expected frontend validation error for invalid rental dates was not shown"
            );
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void examen()
        {
            InitialStepsForRentalCars_UIT();
            seleccionarCocheParaAlquilarPO.SearchCars(0, "Toyota Corolla");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.SearchCars(12, "All");
            seleccionarCocheParaAlquilarPO.AddCarToRentingCart(ModelCar3);
            seleccionarCocheParaAlquilarPO.RemoveCarFromRentingCart(ModelCar);
            seleccionarCocheParaAlquilarPO.RentCars();
            createCocheParaAlquilarPO.fillInformationuser(Name, SurName, DeliveryCarDealer, PaymentMethod);
            createCocheParaAlquilarPO.FillInRentalQuantity(Quantity, carId3);
            createCocheParaAlquilarPO.PressRentCars();
            createCocheParaAlquilarPO.PressOkModalDialog();


            Assert.True(
                createCocheParaAlquilarPO.CheckErrorMessageForMandatoryFields(),
                "Expected frontend validation error was not shown"
            );

        }

    }
}
