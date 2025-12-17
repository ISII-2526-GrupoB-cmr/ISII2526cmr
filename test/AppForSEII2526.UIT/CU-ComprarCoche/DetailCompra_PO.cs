using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_ComprarCoche
{
    public class DetailCompra_PO : PageObject {
        public DetailCompra_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output) {

        }

        public bool CheckPurchaseDetail(string namesurname, string deliveryaddress, DateTime purchasingdate, string totalprice)
        {
            WaitForBeingVisible(By.Id("NameSurname"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(namesurname);
            result = result && _driver.FindElement(By.Id("DeliveryAddress")).Text.Contains(deliveryaddress);
            result = result && _driver.FindElement(By.Id("TotalPrice")).Text.Contains(totalprice);

            var actualPurchaseDate = DateTime.Parse(_driver.FindElement(By.Id("PurchaseDate")).Text);
            result = result && ((actualPurchaseDate - purchasingdate) < new TimeSpan(0, 1, 0));

            return result; 
        }

        public bool CheckListOfCars(List<string[]> expectedPurchaseItems)
        {
            return CheckBodyTable(expectedPurchaseItems, By.Id("PurchasedCars"));
        }


    }
    
    
}
