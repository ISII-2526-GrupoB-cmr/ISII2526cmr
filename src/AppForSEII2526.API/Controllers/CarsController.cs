using AppForSEII2526.API.DTOs.Car;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        //used to enable your controller to access to the database
        private readonly ApplicationDbContext _context;
        //used to log any information when your system is running
        private readonly ILogger<CarsController> _logger;

        public CarsController(ApplicationDbContext context, ILogger<CarsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<Car>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> GetCars()
        {
            IList<Car> cars = await _context.Cars.ToListAsync();
            return Ok(cars);
        }



        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<CocheParaReseñarDTO>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> GetCoche_Para_ReseñarDTO(string? fabricante, string? tipogasoil)
        {
            var coches = await _context.Cars.Include(coche => coche.Model)
                .Where(c => c.Manufacturer.Contains(fabricante) || (fabricante == null) && (c.FuelType.Contains(tipogasoil) || tipogasoil== null)
            )
                .Select(c => new CocheParaReseñarDTO(c.Id, c.Model.Name,
                c.Color, c.FuelType, c.Manufacturer, c.CarClass)).ToListAsync();
            return Ok(coches);
        }
    }
}