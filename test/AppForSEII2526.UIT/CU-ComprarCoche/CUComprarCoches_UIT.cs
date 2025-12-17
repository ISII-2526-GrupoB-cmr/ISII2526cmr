using AppForSEII2526.UIT.Shared;
using OpenQA.Selenium.DevTools.V140.Network;
using OpenQA.Selenium.DevTools.V140.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ComprarCoche
{
    public class CUComprarCoches_UIT: UC_UIT
    {

        private SeleccionarCochesParaComprarPO seleccionarCochesParaComprar_PO;
        private CrearCompra_PO crearCompra_PO;
        private DetailCompra_PO detailCompra_PO;

        private const int carId1 = 1;
        private const string carModel1 = "Toyota Corolla";
        private const string carColor1 = "Gris";
        private const string carPriceForPurchasing1 = "2300000";
        private const string carFuelType1 = "Gasoline";
        private const string carManufacturer1 = "Toyota";

        private const int carId2 = 2;
        private const string carModel2 = "Mazda CX-5";
        private const string carColor2 = "Azul Marino";
        private const string carPriceForPurchasing2 = "2950000";
        private const string carFuelType2 = "Diesel";
        private const string carManufacturer2 = "Mazda";

        private const string name = "Elena";
        private const string surname = "Navarro Martínez";
        private const string deliveryAddress = "Calle Falsa 33";
        private const string quantity1 = "1";
        private const string quantity2 = "2";

        private const string paymentMethod1 = "GooglePay";
        private const string paymentMethod2 = "Visa";

        public CUComprarCoches_UIT(ITestOutputHelper output) : base(output) {
            seleccionarCochesParaComprar_PO = new SeleccionarCochesParaComprarPO(_driver, _output);
            crearCompra_PO = new CrearCompra_PO(_driver, _output);
            detailCompra_PO = new DetailCompra_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("elena@uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchasingCars()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            seleccionarCochesParaComprar_PO.WaitForBeingVisible(By.Id("CreatePurchase"));
            //we click on the menu
            _driver.FindElement(By.Id("CreatePurchase")).Click();
        }

        [Theory]
        [InlineData(carModel1, carColor1, carFuelType1, carManufacturer1, carPriceForPurchasing1, "Gris", "")]
        [InlineData(carModel2, carColor2, carFuelType2, carManufacturer2, carPriceForPurchasing2, "", "Mazda CX-5")]

        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF1_UC1_4_5_filtering(string carModel, string carColor,string carFuelType, string carManufacturer, string carPriceForPurchasing, string filterColor, string filterModel)
        {
            //Arrange
            InitialStepsForPurchasingCars();
            var expectedCars = new List<string[]> { new string[] { carModel, carColor, carPriceForPurchasing, carFuelType, carManufacturer }, };

            //Act
            seleccionarCochesParaComprar_PO.SearchCars(filterColor, filterModel);

            //Assert
            Assert.True(seleccionarCochesParaComprar_PO.CheckListOfCars(expectedCars));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF2_UC1_6_PurchasingNotavailable()
        {
            //Arrange
            InitialStepsForPurchasingCars();

            //Act
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);

            seleccionarCochesParaComprar_PO.RemoveCarFromPurchasingCart(carModel1);

            //Assert
            Assert.True(seleccionarCochesParaComprar_PO.PurchasingNotAvailable());
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        // compilar dbo.inicializacion.data.sinstock.sql
        public void UC1_AF0_UC1_3_PurchaseNoStock()
        {
            //Arrange
            InitialStepsForPurchasingCars();
            var expectedError = "Errors: No hay stock en el concesionario";

            //Act
            

            //Assert
            Assert.True(seleccionarCochesParaComprar_PO.CheckMessageError(expectedError));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF3_UC1_7_Actualizar_TotalPrice()
        {
            //Arrange
            InitialStepsForPurchasingCars();
            var totalPriceInicio="";
            var totalPriceFinal = "";

            var expectedInicio = "7550000";
            var expectedFinal = "4600000";
            
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel2);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            crearCompra_PO.FillInPurchaseQuantity(quantity2, carModel1);
            crearCompra_PO.FillInPurchaseQuantity(quantity1, carModel2);

            totalPriceInicio = crearCompra_PO.ReturnTotalPrice();
          
            //Act
            crearCompra_PO.VolverAlSelect();
            seleccionarCochesParaComprar_PO.RemoveCarFromPurchasingCart(carModel2);
            seleccionarCochesParaComprar_PO.PurchaseCars();
            totalPriceFinal = crearCompra_PO.ReturnTotalPrice();


            //Assert
            Assert.True(crearCompra_PO.CheckTotalPrices(totalPriceFinal, expectedFinal, totalPriceInicio, expectedInicio));
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF5_12_GuardarDatos()
        {
            //Arrange
            InitialStepsForPurchasingCars();

            string carModelSinEspacios = carModel1.Replace(" ", "");
            string expectedData = name + surname + deliveryAddress + paymentMethod2 + quantity1;

            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel2);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            crearCompra_PO.FillInPurchaseInfo(name, surname, deliveryAddress, paymentMethod2);
            crearCompra_PO.FillInPurchaseQuantity(quantity1, carModel1);

            //Act
            crearCompra_PO.VolverAlSelect();
            seleccionarCochesParaComprar_PO.RemoveCarFromPurchasingCart(carModel2);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            string actualData = crearCompra_PO.ReturnData(carModel1);

            //Assert
            Assert.True(crearCompra_PO.CheckDatosGuardados(expectedData, actualData));

        }

        [Theory]
        [InlineData(name, surname, deliveryAddress, paymentMethod1, quantity1)]
        [InlineData(name, surname, deliveryAddress, paymentMethod2, quantity1)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_1_2_BasicFlow (string name, string surname, string deliveryAddress, string paymentMethod, string quantity)
        {
            //Arrange

            var expectedPurchaseItems = new List<string[]> { new string[] { carModel1, carColor1, carPriceForPurchasing1, quantity } };
            var namesurname = name + " " + surname;

            //Act
            InitialStepsForPurchasingCars();
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            crearCompra_PO.FillInPurchaseInfo(name, surname, deliveryAddress, paymentMethod);
            crearCompra_PO.FillInPurchaseQuantity(quantity, carModel1);
            
            crearCompra_PO.PressPurchaseYourCars();
            crearCompra_PO.PressOkModalDialog();

            //Assert
            Assert.True(detailCompra_PO.CheckPurchaseDetail(namesurname, deliveryAddress, DateTime.Now, carPriceForPurchasing1), "Error: detail purchase is not as expected");
            Assert.True(detailCompra_PO.CheckListOfCars(expectedPurchaseItems), "Error, purchase items are not as expected");

        }

        [Theory]
        [InlineData("", "Navarro Martínez", "Calle Albacete 33","GooglePay", "1", "The CustomerName field is required.")]
        [InlineData("Elena", "", "Calle Albacete 33", "GooglePay", "1", "The CustomerSurname field is required.")]
        [InlineData("Elena", "Navarro Martínez", "", "GooglePay", "1", "The DeliveryAddress field is required.")]  
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF4_8_9_10_testingErrorsMandatorydata(string name, string surname, string deliveryaddres, string paymentmethod, string quantity, string expectedMessageError)
        {
            //Arrange
            InitialStepsForPurchasingCars();
            //Act
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            crearCompra_PO.FillInPurchaseInfo(name, surname, deliveryaddres, paymentmethod);
            crearCompra_PO.FillInPurchaseQuantity(quantity, carModel1);

            crearCompra_PO.PressPurchaseYourCars();
            //crearCompra_PO.PressOkModalDialog();

            //Assert
            Assert.True(crearCompra_PO.CheckValidationError(expectedMessageError), $"Expected error: {expectedMessageError}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF4_11_testingErrorsMandatorydataQuantity()
        {
            //Arrange
            InitialStepsForPurchasingCars();
            var expectedMessageError = "Errors: (*) Error! You cannot select quantity 0 for purchase";
            var quantity0 = "0";
            //Act
            seleccionarCochesParaComprar_PO.AddMovieToPurchasingCart(carModel1);
            seleccionarCochesParaComprar_PO.PurchaseCars();

            crearCompra_PO.FillInPurchaseInfo(name, surname, deliveryAddress, paymentMethod1);
            crearCompra_PO.FillInPurchaseQuantity(quantity0, carModel1);

            crearCompra_PO.PressPurchaseYourCars();
            crearCompra_PO.PressOkModalDialog();

            //Assert
            Assert.True(crearCompra_PO.CheckValidationErrorQuantity(expectedMessageError), $"Expected error: {expectedMessageError}");
        }
    }
}

