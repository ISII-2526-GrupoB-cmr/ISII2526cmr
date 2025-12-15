using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;


namespace AppForSEII2526.UIT.CU_AlquilarCoche
{
    public class SeleccionarCocheParaAlquilarPO : PageObject
    {
        private By Inputprice = By.Id("Inputprice");
        private By selectModel = By.Id("selectModel");
        private By buttonSearchCars = By.Id("SearchCars");
        private By tableOfCarsBy = By.Id("TableOfCars");
        private By buttonRentCar = By.Id("rentalcarButton");

        public SeleccionarCocheParaAlquilarPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
            IWebElement elementPrice= _driver.FindElement(Inputprice);

            IWebElement selectDropDown = _driver.FindElement(selectModel);
            SelectElement select = new SelectElement(selectDropDown);

            elementPrice.Click();
            elementPrice.Clear();


        }

        public void SearchCars(int price, string model)
        {
            WaitForBeingVisible(Inputprice);
            //Se introduce el color
            _driver.FindElement(Inputprice).SendKeys(price.ToString());
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

        public void AddCarToRentingCart(string carmodel)
        {
            WaitForBeingClickable(By.Id("carToRent_" + carmodel));

            _driver.FindElement(By.Id("carToRent_" + carmodel)).Click();
        }

        public void RemoveCarFromRentingCart(string carmodel)
        {
            WaitForBeingClickable(By.Id("removecar_" + carmodel));
            _driver.FindElement(By.Id("removecar_" + carmodel)).Click();
        }

        public void RentCars()
        {
            WaitForBeingClickable(buttonRentCar);
            _driver.FindElement(buttonRentCar).Click();
        }



    }
}
