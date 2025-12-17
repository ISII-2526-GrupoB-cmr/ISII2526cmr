using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_AlquilarCoche
{
    public class CreateCocheParaAlquilarPO : PageObject
    {


        public CreateCocheParaAlquilarPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }
        private By Name = By.Id("Name");
        private By Surname = By.Id("Surname");
        private By Address = By.Id("Address");
        private By Paymentmethod = By.Id("PaymentMethod");
        private By buttonRentCar = By.Id("rentalcarButton");
       
        private By StartDate = By.Id("StartDate");
        private By EndDate = By.Id("EndDate");

        private IWebElement _name() => _driver.FindElement(Name);
        private IWebElement _surname() => _driver.FindElement(Surname);
        private IWebElement _address() => _driver.FindElement(Address);
        private IWebElement _paymentmethod() => _driver.FindElement(Paymentmethod);


        public void fillInformationuser(string name, string surname, string address, string paymentmethod)
        {
            WaitForBeingVisible(Name);
            WaitForBeingVisible(Surname);
            WaitForBeingVisible(Address);

            _name().Clear();
            _surname().Clear();
            _address().Clear();
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
            WaitForBeingClickable(By.Id($"quantity_{carId}"));
            var quantityInput = _driver.FindElement(By.Id($"quantity_{carId}"));
            quantityInput.Clear();
            quantityInput.SendKeys(quantity);
        }



        public void PressRentCars()
        {
            _driver.FindElement(By.Id("rentalcarButton")).Click();
        }

        public void PressModifyCars()
        {
            _driver.FindElement(By.Id("Modifycars")).Click();
        }

        public bool CheckListOfRentalItems(List<string[]> expectedRentalItems)
        {
            return CheckBodyTable(expectedRentalItems, By.Id("TableOfRentalItems"));
        }
        public void SetRentalDates(DateTime startDate, DateTime endDate)
        {
            WaitForBeingVisible(By.Id("StartDate"));
            WaitForBeingVisible(By.Id("EndDate"));

            var startDateInput = _driver.FindElement(By.Id("StartDate"));
            startDateInput.Clear();
            startDateInput.SendKeys(startDate.ToString("yyyy-MM-dd"));

            var endDateInput = _driver.FindElement(By.Id("EndDate"));
            endDateInput.Clear();
            endDateInput.SendKeys(endDate.ToString("yyyy-MM-dd"));
        }


        public bool CheckValidationError(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);
        }
        public bool CheckErrorMessageForMandatoryFields()
        {
            Thread.Sleep(1000);
            var error = _driver.FindElement(By.Id("ErrorsShown"));
            Thread.Sleep(1000);
            return !string.IsNullOrWhiteSpace(error.Text.Replace("Errors:", "").Trim()
         );
        }
        public bool CheckErrorMessageForInvalidDates()
        {
            Thread.Sleep(3000);
            var error = _driver.FindElement(By.Id("DateErrorsShown"));
            Thread.Sleep(3000);
            return !string.IsNullOrWhiteSpace(error.Text.Replace("Date Errors:", "").Trim()
         );
        }

    }
}
