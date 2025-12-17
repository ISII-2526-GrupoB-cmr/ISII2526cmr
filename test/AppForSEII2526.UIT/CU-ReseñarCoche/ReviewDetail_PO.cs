//ReviewDetail_PO.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSeII2526.UIT.CU_ReseñarCoche
{
    public class ReviewDetail_PO : PageObject
    {
        public ReviewDetail_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReviewDetail(string username, string country, DateTime created, Enum drivertype)
        {
            Thread.Sleep(1000);
            bool result = true;
            result = result && _driver.FindElement(By.Id("NameSurname")).Text.Contains(username);
            result = result && _driver.FindElement(By.Id("Country")).Text.Trim().ToLower().Contains(country.ToLower());
            var actualReviewDate = DateTime.Parse(_driver.FindElement(By.Id("Created")).Text);
            result = result && ((actualReviewDate - actualReviewDate) < new TimeSpan(0, 1, 0));
            result = result && _driver.FindElement(By.Id("DriverType")).Text.Contains(drivertype.ToString());




            return result;

        }

        public bool CheckListOfCars(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("ReviewCars"));
        }
    }
}