using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.Car;

namespace AppForSEII2526.UT.CarsControllersTest
{
    internal class GetCarsForReview
    {
        public GetCarsForReview_test() {

            var models = new List<Model>
            {
                new Model("Modelo A"),
                new Model("Modelo B"),
                new Model("Modelo C"),

            };
            var cars = new List<Car>
            {
                new Car("10", ),
                new Car("DEF456", "Blue", 2019, models[1]),
                new Car("GHI789", "Black", 2021, models[2]),
                new Car("JKL012", "White", 2018, models[0]),
            };

            ApplicationUser user = new ApplicationUser("1", "César", "Minglanilla Díaz", "cesar@uclm.es");
            
             

            var review= new Review()

        }
    }
}
