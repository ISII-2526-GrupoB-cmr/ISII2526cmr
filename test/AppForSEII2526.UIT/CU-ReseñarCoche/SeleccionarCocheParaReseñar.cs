using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ReseñarCoche
{
    internal class SeleccionarCocheParaReseñar : PageObject
    {

        private By selectManufacturer = By.Id("selectManufacturer");
        private By selectFueltype = By.Id("selectFueltype");
        private By _reviewButton = By.Id("ReviewCarButton");
        private By searchCars = By.Id("searchCars");
        private By _tableOfCars = By.Id("TableOfCars");
        private By _modalBy = By.Id("DialogOKSaveDelete");


        public SeleccionarCocheParaReseñar(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
            
        }

        public void Buscarcoche(string manufacturer, string fueltype)
        {
            WaitForBeingVisible(selectManufacturer);
            WaitForBeingVisible(selectFueltype);
            var manuInput = _driver.FindElement(selectManufacturer);
            manuInput.SendKeys(manufacturer);



            var fuelInput = _driver.FindElement(selectFueltype);
            fuelInput.SendKeys(fueltype);

            _driver.FindElement(searchCars).Click();
        }

        public void SelectCarsForReview(List<string> models)
        {
           
            foreach (var model in models)
            {
                By botonAdd = By.Id("ReviewCar" + model);
                WaitForBeingVisible(_reviewButton);
                _driver.FindElement(_reviewButton).Click();
            }
        }

        public void ReviewCars()
        {
            WaitForBeingClickable(_reviewButton);
            _driver.FindElement(_reviewButton).Click();
        }




        public bool CheckListOfCars(List<string[]> expectedCars)
        {

            return CheckBodyTable(expectedCars, _tableOfCars);
        }
        

        public bool CheckReviewCarsDisabled()
        {
            //devolvemos true si el botón está deshabilitado
            return !_driver.FindElement(_reviewButton).Enabled;
        }

        public void ModifyReviewCart(string model)
        {
            By botonRemove = By.Id("removeCar_" + model);
            WaitForBeingClickable(By.Id($"removeCar_{model}"));
            _driver.FindElement(By.Id($"removeCar_{model}")).Click();

        }


        public void ClickReviewButton()
        {
            WaitForBeingClickable(_reviewButton);
            _driver.FindElement(_reviewButton).Click();
        }

        public bool CheckMessageErrorNotAvailableCarsForReview(string expectedError)
        {
            return _driver.PageSource.Contains(expectedError);

        }




        public bool CheckMessageError(string expectedError)
        {
            return CheckModalBodyText(expectedError, _modalBy);
        }



    }

}
    

   

