using OpenQA.Selenium.DevTools.V140.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ComprarCoche
{
    public class CrearCompra_PO : PageObject
    {
        private By inputName = By.Id("Name");
        private By inputSurname = By.Id("Surname");
        private By inputDeliveryAddress = By.Id("DeliveryAddress");
        private By inputPaymentMethod = By.Id("PaymentMethod");
        private By errorShownBy = By.Id("ErrorsShown");

        private IWebElement nameElement() => _driver.FindElement(inputName);
        private IWebElement surnameElement() => _driver.FindElement(inputSurname);

        private IWebElement deliveryAddressElement() => _driver.FindElement(inputDeliveryAddress);
        private IWebElement paymentMethodElement() => _driver.FindElement(inputPaymentMethod);

        public CrearCompra_PO(IWebDriver driver, ITestOutputHelper output) : base (driver, output)
        {

        }

        public void FillInPurchaseInfo(string name, string surname, string deliveryAddress, string paymentMethod)
        {
            WaitForBeingClickable(inputName);
            nameElement().SendKeys(name);
            surnameElement().SendKeys(surname);
            deliveryAddressElement().SendKeys(deliveryAddress);

            //create select element object 
            SelectElement selectElement = new SelectElement(paymentMethodElement());

            //select Action from the dropdown menu
            selectElement.SelectByText(paymentMethod);
        }

        public void FillInPurchaseQuantity(string quantity, string carModel)
        {
            //como es un inputNumber, con los metodos normale no me metia la cantidad
            //por eso uso los metodos del javascript, sino me era imposible rellenarlos
            string carModelSinEspacios = carModel.Replace(" ", "");
            By quantityBy = By.Id("quantity_" + carModelSinEspacios);

            WaitForBeingVisible(quantityBy);
            var element = _driver.FindElement(quantityBy);

            //uso el js.ExecuteScript para poner la cantidad
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].value = arguments[1]; arguments[0].dispatchEvent(new Event('change'));",element, quantity);

        }

        public string ReturnTotalPrice()
        {
            WaitForBeingVisible(inputPaymentMethod);
            return _driver.FindElement(By.Id("TotalPrice")).Text;
        }

        public void VolverAlSelect()
        {
            _driver.FindElement(By.Id("ModifyCars")).Click();
        }
        public bool CheckTotalPrices(string totalPriceFinal, string expectedFinal, string totalPriceInicio, string expectedInicio)
        {
            return (totalPriceFinal ==  expectedFinal && totalPriceInicio == expectedInicio);
        }

        public void PressPurchaseYourCars()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("ModifyCars")).Click();
        }

        public bool CheckListOfPurchaseItems(List<string[]> expectedPurchaseItems)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("TableOfPurchaseItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

        public bool CheckValidationErrorQuantity(string errorMessage)
        {
            WaitForBeingVisible(errorShownBy);
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text == errorMessage;
        }

        public string ReturnData(string carModel)
        {
            string carModelSinEspacios = carModel.Replace(" ", "");

            string name = _driver.FindElement(inputName).GetAttribute("value");
            string surname = _driver.FindElement(inputSurname).GetAttribute("value");
            string deliveryAddress = _driver.FindElement(inputDeliveryAddress).GetAttribute("value");
            string paymentmethod = _driver.FindElement(inputPaymentMethod).GetAttribute("value");
            string quantity = _driver.FindElement(By.Id("quantity_" + carModelSinEspacios)).GetAttribute("value");

            return name + surname + deliveryAddress + paymentmethod + quantity;
        }

        public bool CheckDatosGuardados(string expectedDatos, string actualDatos)
        {
            
            return expectedDatos == actualDatos;

        }
    }
}
