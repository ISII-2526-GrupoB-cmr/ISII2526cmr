//CreateReview_PO


using OpenQA.Selenium.DevTools.V140.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ReseñarCoche
{
    internal class CreateReview_PO : PageObject
    {

        private By _username = By.Id("Username");
        private By _country = By.Id("Country");
        private By _driverType = By.Id("DriverType");

        private By _modifyBttn = By.Id("ModifyReviewCart");

        private By _submitReviewbttn = By.Id("Submit");
        private By _reviewTable = By.Id("TableOfReviewItems");


        private IWebElement username() => _driver.FindElement(_username);
        private IWebElement country() => _driver.FindElement(_country);
        private IWebElement driverType() => _driver.FindElement(_driverType);


        public CreateReview_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }


        public void RellenarReview(string username, string country, Enum driverType)
        {
            WaitForBeingVisible(_username);

            this.username().Clear();
            this.username().SendKeys(username);
            this.country().Clear();
            this.country().SendKeys(country);
            this.driverType().SendKeys(driverType.ToString());
        }

        public void RellenarDetailReview(string reviewDescription, int Reviewrating, string model)
        {
            var descriptionInput = _driver.FindElement(
                By.Id($"description_{model}")
            );
            descriptionInput.Clear();
            descriptionInput.SendKeys(reviewDescription);

            var ratingSelect = new SelectElement(
                _driver.FindElement(By.Id($"rating_{model}"))
            );
            ratingSelect.SelectByValue(Reviewrating.ToString());
        }



        public void PulsarSubmitReview()
        {
            _driver.FindElement(_submitReviewbttn).Click();
        }

        public void PulsarModifyCar()
        {

            _driver.FindElement(_modifyBttn).Click();
        }




        public bool ComprobarReviewItemsTable(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, _reviewTable);
        }

        public bool CheckErrorMessageForMandatoryFields()
        {

            Thread.Sleep(1000);


            var errores = _driver.FindElements(By.ClassName("validation-message"));


            var sumarioErrores = _driver.FindElements(By.ClassName("validation-errors"));


            return errores.Any(e => !string.IsNullOrWhiteSpace(e.Text)) ||
                   sumarioErrores.Any(e => !string.IsNullOrWhiteSpace(e.Text));
        }

    }
}
