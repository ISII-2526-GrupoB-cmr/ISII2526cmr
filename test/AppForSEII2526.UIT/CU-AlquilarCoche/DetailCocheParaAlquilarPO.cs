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

        public bool CheckDetailParaAlquilar(string name, string surname, string address, string paymentmethod, DateTime rentalDate, DateTime startdate, DateTime enddate, string totalprice)
        {
            bool comprobado = true;
            WaitForBeingVisible(By.Id("RentalTotalPrice"));

            Thread.Sleep(1000);

            var nameSurname = _driver.FindElement(By.Id("NameSurname")).Text.Trim();
            var delivery = _driver.FindElement(By.Id("DeliveryAddress")).Text.Trim();
            var payment = _driver.FindElement(By.Id("PaymentMethod")).Text.Trim();
            var total = _driver.FindElement(By.Id("TotalPrice")).Text.Trim(); // ejemplo: "85 €"

            // Name + Surname
            if (!nameSurname.Contains(name) || !nameSurname.Contains(surname)) return false;

            // Address
            if (!delivery.Contains(address)) return false;

            // Payment method (si en pantalla sale "Visa", "Paypal", etc.)
            if (!payment.Contains(paymentmethod)) return false;

            // Total (en pantalla viene con "€")
            if (!total.Contains(totalprice)) return false;

            // Periodo (está formateado dd/MM/yyyy - dd/MM/yyyy)
            var expectedPeriod = $"{startdate:dd/MM/yyyy} - {enddate:dd/MM/yyyy}";
            var period = _driver.FindElement(By.Id("RentalPeriod")).Text.Trim();
            if (!period.Contains(expectedPeriod)) return false;


            return comprobado;

        }
    }
}
