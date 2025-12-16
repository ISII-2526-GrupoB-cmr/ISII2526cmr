using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSeII2526.UIT.ReviewDetail
{
    public class ReviewDetail_PO : PageObject
    {
        public  ReviewDetail_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool CheckReviewDetail(string username, string country, DateTime created, Enum drivertype)
        {
            WaitForBeingVisible(By.Id("TotalPrice"));
            bool result = true;
            result = result && _driver.FindElement(By.Id("Username")).Text.Contains(username);
            result = result && _driver.FindElement(By.Id("Country")).Text.Contains(country);
            result = result && _driver.FindElement(By.Id("Created")).Text.Contains(created.ToString("g"));
            result = result && _driver.FindElement(By.Id("DriverType")).Text.Contains(drivertype.ToString());




            return result;

        }

        public bool CheckListOfCars(List<string[]> expectedReviewItems)
        {
            return CheckBodyTable(expectedReviewItems, By.Id("ReviewCars"));
        }
    }
}