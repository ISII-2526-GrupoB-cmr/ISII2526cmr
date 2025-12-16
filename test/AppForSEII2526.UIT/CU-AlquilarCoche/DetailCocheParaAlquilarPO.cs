using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_AlquilarCoche
{
    public class DetailCocheParaAlquilarPO : PageObject
    {
        public DetailCocheParaAlquilarPO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {

        }

        public bool CheckDetailParaAlquilar(string name, string surname, string address, string paymentmethod,DateTime rentalDate, DateTime startdate, DateTime enddate, string totalprice)
        {
            bool comprobado = true;
            WaitForBeingVisible(By.Id("RentalTotalPrice"));

            comprobado &= _driver.FindElement(By.Id("Name")).Text.Contains(name);
            comprobado &= _driver.FindElement(By.Id("Surname")).Text.Contains(surname);
            comprobado &=  _driver.FindElement(By.Id("Address")).Text.Contains(address);
            comprobado &= _driver.FindElement(By.Id("PaymentMethod")).Text.Contains(paymentmethod);
            comprobado &= _driver.FindElement(By.Id("RentalDate")).Text.Contains(rentalDate.ToString("dd/MM/yy"));

            var rentalPeriodText = _driver.FindElement(By.Id("RentalPeriod")).Text;
            comprobado &= rentalPeriodText.Contains(startdate.ToString("dd/MM/yyyy"));
            comprobado &= rentalPeriodText.Contains(enddate.ToString("dd/MM/yyyy"));
            comprobado &= _driver.FindElement(By.Id("TotalPrice")).Text.Contains(totalprice);


            return comprobado;

        }
    }
}
