//CU_Review_UIT.cs

using AppForSeII2526.UIT.CU_ReseñarCoche;
using AppForSEII2526.UIT.CU_AlquilarCoche;
using AppForSEII2526.UIT.Shared;
using AppForSEII2526.Web.API;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_ReseñarCoche
{
    public class CU_Review_UIT : UC_UIT
    {
        public CU_Review_UIT(ITestOutputHelper output) : base(output)
        {
            seleccionarCocheParaReseñar = new SeleccionarCocheParaReseñar(_driver, output);
            createReview_PO = new CreateReview_PO(_driver, output);
            reviewDetail_PO = new ReviewDetail_PO(_driver, output);

        }

        private const int carId = 1;
        private const string ModelCar = "Toyota Corolla";
        private const string manufactutrer = "Toyota";
        private const string fueltype = "Gasoline";
        private const string color = "Gris";
        private const string description = "Reseña para Toyota Corolla";
        private const int rating = 3;

        private const int carId2 = 2;
        private const string ModelCar2 = "Mazda CX-5";
        private const string manufacturer2 = "Mazda";
        private const string fueltype2 = "Diesel";
        private const string color2 = "Azul Marino";
        private const string description2 = "Reseña Mazda CX-5";
        private const int rating2 = 4;


        private const string username = "elena@uclm.es";
        private const string country = "Spain";
        private const DriverType driverType = DriverType.Experto;

        private SeleccionarCocheParaReseñar seleccionarCocheParaReseñar;
        private CreateReview_PO createReview_PO;
        private ReviewDetail_PO reviewDetail_PO;


        private void PrimerosPasosReviewCar()
        {
            Initial_step_opening_the_web_page();

            Thread.Sleep(500);

            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.Url.Contains("review"));
            }
            catch (WebDriverTimeoutException)
            {
                _driver.Navigate().GoToUrl(new Uri(_driver.Url).GetLeftPart(UriPartial.Authority) + "/review/seleccionarcochereview");
            }

        }

        [Theory]
        [InlineData(ModelCar, username, country, "Experto", rating, description)]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_4_BasicFlow_ReviewCar(string model, string username, string country, string driverType, int rating, string description)
        {
            PrimerosPasosReviewCar();
            var parsedDriverType = Enum.Parse<DriverType>(driverType);

            var expectedReview = new List<string[]>
{
                new string[] {
                    model,
                    manufactutrer,
                    fueltype,
                    color,
                    rating.ToString(),
                    description
                }
            };

            seleccionarCocheParaReseñar.SelectCarsForReview(model);
            seleccionarCocheParaReseñar.ReviewCars();

            createReview_PO.RellenarReview(username, country, parsedDriverType);
            createReview_PO.RellenarDetailReview(description, rating, model);
            createReview_PO.PulsarSubmitReview();
            createReview_PO.PressOkModalDialog();

            Assert.True(reviewDetail_PO.CheckReviewDetail(username, country, DateTime.Now, parsedDriverType));
            Assert.True(reviewDetail_PO.CheckListOfCars(expectedReview), "Error, los items de review no son los esperados");
        }

        [Theory]
        [InlineData(ModelCar, manufactutrer, fueltype, color, "Toyota", "")]
        [InlineData(ModelCar2, manufacturer2, fueltype2, color2, "", "Diesel")]
        [Trait("LevelTesting", "Funcional testing")]
        public void UC4_AF0_FiltarFabricanteCombustible(string model, string manufacturer, string fueltype, string color, string filtroManufacturer, string filtroFueltype)
        {

            PrimerosPasosReviewCar();

            var expectedCars = new List<string[]>
            {
               new string[] { model.Trim(), manufacturer.Trim(), fueltype.Trim(), color.Trim() }

            };

            seleccionarCocheParaReseñar.Buscarcoche(filtroManufacturer, filtroFueltype);

            Assert.True(seleccionarCocheParaReseñar.CheckListOfCars(expectedCars));

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF1_4_CocheNoSeleccionado()
        {

            PrimerosPasosReviewCar();
            Assert.True(seleccionarCocheParaReseñar.CheckReviewCarsDisabled());

        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF2_5_ModificarCarritoReseña()
        {
            PrimerosPasosReviewCar();



            seleccionarCocheParaReseñar.SelectCarsForReview(ModelCar);
            seleccionarCocheParaReseñar.SelectCarsForReview(ModelCar2);
            seleccionarCocheParaReseñar.ReviewCars();

            createReview_PO.RellenarReview(username, country, driverType);
            createReview_PO.RellenarDetailReview(description, rating, ModelCar);
            createReview_PO.RellenarDetailReview(description2, rating2, ModelCar2);
            createReview_PO.PulsarModifyCar();

            seleccionarCocheParaReseñar.PularQuitarCoche(ModelCar2);
            seleccionarCocheParaReseñar.ReviewCars();

            var expectedReview = new List<string[]>
            {
                new string[] { ModelCar, fueltype, manufactutrer, color }

            };

            Assert.True(createReview_PO.ComprobarReviewItemsTable(expectedReview));

        }

        [Theory]
        [InlineData(username, " ", "Experto")]
        [InlineData("", country, "Experto")]


        [Trait("LevelTesting", "Funcional Testing")]
        public void UC4_AF3_6_DatosReviewInvalidos(string username, string country, string driverType)
        {
            PrimerosPasosReviewCar();
            seleccionarCocheParaReseñar.SelectCarsForReview(ModelCar);
            seleccionarCocheParaReseñar.ReviewCars();
            var parsedDriverType = Enum.Parse<DriverType>(driverType);
            createReview_PO.RellenarReview(username, country, parsedDriverType);
            createReview_PO.RellenarDetailReview(description, rating, ModelCar);
            createReview_PO.PulsarSubmitReview();
            Thread.Sleep(500);
            Assert.True(createReview_PO.CheckErrorMessageForMandatoryFields(),
                "Error: The error message for mandatory fields is not shown as expected.");
        }

        [Fact]
        [Trait("Level Testing", "Functional Testing")]

        public void UC4_AF5_7_ModificarCochesSeleccionados()
        {
            PrimerosPasosReviewCar();

            seleccionarCocheParaReseñar.SelectCarsForReview(ModelCar);
            seleccionarCocheParaReseñar.SelectCarsForReview(ModelCar2);
            seleccionarCocheParaReseñar.ReviewCars();

            createReview_PO.RellenarReview(username, country, driverType);
            createReview_PO.PulsarModifyCar();

            seleccionarCocheParaReseñar.PularQuitarCoche(ModelCar);
            seleccionarCocheParaReseñar.ReviewCars();

            var expectedCars = new List<string[]>
            {
               new string[] { ModelCar2, fueltype2, manufacturer2, color2 }
            };

            Assert.True(createReview_PO.ComprobarReviewItemsTable(expectedCars));


        }


    }
}