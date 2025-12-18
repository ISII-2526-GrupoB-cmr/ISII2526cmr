using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.CU_ComprarCoche
{
    public class SeleccionarCochesParaComprarPO : PageObject
    {
        private By inputColor = By.Id("inputColor");
        private By selectModel = By.Id("selectModel");
        private By buttonSearchCars = By.Id("searchCars");
        private By tableOfCarsBy = By.Id("TableOfCars");
        private By buttonPurchaseCars = By.Id("purchaseCarButton");
        private By errorShownBy = By.Id("ErrorsShown");


        public SeleccionarCochesParaComprarPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
            //IWebElement elementColor = _driver.FindElement(inputColor);

            //IWebElement selectDropDown = _driver.FindElement(selectModel);
            //SelectElement select = new SelectElement(selectDropDown);

            //elementColor.Click();
            //elementColor.Clear();
            //colorFilter = "Gris";


        }
        public void SearchCars(string color, string model)
        {
            WaitForBeingVisible(inputColor);
            _driver.FindElement(inputColor).Clear();
            //Se introduce el color
            _driver.FindElement(inputColor).SendKeys(color);
            //Con todo esto se selecciona el modelo
            if (model == "") model = "All";
            SelectElement selectElement = new SelectElement(_driver.FindElement(selectModel));
            selectElement.SelectByText(model);

            _driver.FindElement(buttonSearchCars).Click();
        }

        public bool CheckListOfCars(List<string[]> expectedCars)
        {
            return CheckBodyTable(expectedCars, tableOfCarsBy);
        }

        public bool CheckMessageError(string errorMessage)
        {
            WaitForBeingVisible(errorShownBy);
            IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text == errorMessage;
        }

        public void AddMovieToPurchasingCart(string carModel)
        {
            WaitForBeingClickable(By.Id("carToPurchase_" + carModel));

            _driver.FindElement(By.Id("carToPurchase_" + carModel)).Click();
        }

        public void RemoveCarFromPurchasingCart(string carModel)
        {
            WaitForBeingClickable(By.Id("removeCar_" + carModel));
            _driver.FindElement(By.Id("removeCar_" + carModel)).Click();
        }

        public void PurchaseCars()
        {
            WaitForBeingClickable(buttonPurchaseCars);
            _driver.FindElement(buttonPurchaseCars).Click();
        }

        public bool PurchasingNotAvailable()
        {
            //the button is not Displayed=hidden

            return _driver.FindElement(buttonPurchaseCars).Displayed == false;
        }

    }
}
