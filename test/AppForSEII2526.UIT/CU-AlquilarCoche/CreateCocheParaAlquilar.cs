using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_AlquilarCoche
{
    public class CreateCocheParaAlquilar : PageObject
    {

       
        public CreateCocheParaAlquilar(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }
        private By Name = By.Id("Name");
        private By Surname = By.Id("Surname");
        private By Address = By.Id("Address");
        private By Paymentmethod = By.Id("PaymentMethod");
        private By buttonRentCar = By.Id("rentalcarButton");

        private IWebElement _name() => _driver.FindElement(Name);
        private IWebElement _surname() => _driver.FindElement(Surname);
        private IWebElement _address() => _driver.FindElement(Address);
        private IWebElement _paymentmethod() => _driver.FindElement(Paymentmethod);


        public void fillInformationuser(string name,string surname, string address, string paymentmethod) {
            WaitForBeingVisible(Name);
            WaitForBeingVisible(Surname);
            WaitForBeingVisible(Address);
            _name().SendKeys(name);
            _surname().SendKeys(surname);
            _address().SendKeys(address);

            //create select element object 
            SelectElement selectElement = new SelectElement(_paymentmethod());

            //select Action from the dropdown menu
            selectElement.SelectByText(paymentmethod);

        }

        public void FillInRentalQuantity(string quantity, int carId)
        {
            WaitForBeingClickable(By.Id("_"+ quantity));

           
            _driver.FindElement(By.Id("description_" + carId)).SendKeys(quantity);
        }


        public void PressRentCars()
        {
            _driver.FindElement(By.Id("Submit")).Click();
        }

        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("Modifycars")).Click();
        }

        public bool CheckListOfRentalItems(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }

        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }

    }
}
