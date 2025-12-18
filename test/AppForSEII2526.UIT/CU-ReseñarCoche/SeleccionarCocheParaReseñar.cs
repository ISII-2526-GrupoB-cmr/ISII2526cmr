//SeleccionarCocheParaReseñar_PO.cs

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
        private By _modifyCar = By.Id("ModifyReviewCart");


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

        public void SelectCarsForReview(string model)
        {
            var addButtonId = By.Id("carToReview_" + model);

            WaitForBeingClickable(addButtonId);
            _driver.FindElement(addButtonId).Click();


        }

        public void BuscarFueltype(string fueltype) {
            WaitForBeingVisible(selectFueltype);
            var fuelInput = _driver.FindElement(selectFueltype);
            fuelInput.SendKeys(fueltype);
            _driver.FindElement(searchCars).Click();
        
        
        }

        public void BorrarManufacturer(string manufacturer) {
            WaitForBeingVisible(selectManufacturer);
            var manuInput = _driver.FindElement(selectManufacturer);
            manuInput.Clear();
            _driver.FindElement(searchCars).Click();



        }

        public void BuscarManufacturer(string manufacturer)
        {
            WaitForBeingVisible(selectManufacturer);
            var manuInput = _driver.FindElement(selectManufacturer);
            manuInput.SendKeys(manufacturer);
            _driver.FindElement(searchCars).Click();


        }


        public void PularQuitarCoche(string model)
        {
            var quitarButtonId = $"removeCar_{model}";
            WaitForBeingClickable(By.Id(quitarButtonId));
            _driver.FindElement(By.Id(quitarButtonId)).Click();
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


            Thread.Sleep(1000); // Solo para probar si es un tema de tiempo
            var botonDesactivado = _driver.FindElement(_reviewButton);
            return !botonDesactivado.Displayed;
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



