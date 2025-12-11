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
    public class SeleccionarCochesParaComprarPO: PageObject
    {
        private By inputColor = By.Id("inputColor");
        private By selectModel = By.Id("selectModel");
        private By buttonSearchCars = By.Id("SearchCars");
        private By tableOfCarsBy = By.Id("TableOfMovies");
        private string colorFilter;
        public SeleccionarCochesParaComprarPO(IWebDriver driver, ITestOutputHelper output): base(driver, output) { 
            IWebElement elementColor = _driver.FindElement(inputColor);

            IWebElement selectDropDown = _driver.FindElement(selectModel);
            SelectElement select = new SelectElement(selectDropDown);

            elementColor.Click();
            elementColor.Clear();
            colorFilter = "Gris";

           
        }
        public void SearchCars(string color, string model)
        {
            WaitForBeingVisible(inputColor);
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
    }
}
