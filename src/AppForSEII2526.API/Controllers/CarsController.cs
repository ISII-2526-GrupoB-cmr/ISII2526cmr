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
        [ProducesResponseType(typeof(IList<CocheParaAlquilarDTO>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> GetCoche_Para_AlquilarDTO(float precio, string? modelo)
        {
            var coches = await _context.Cars.Include(Car => Car.Model)
              .Where(c => c.PurchasePrice.Equals(precio) || (precio == null) && (c.Model.Name.Contains(modelo) || modelo == null))
            
                .Select(c => new CocheParaAlquilarDTO(c.Id, c.Model.Name,
        c.Color, c.FuelType, c.Manufacturer, c.RentingPrice)).ToListAsync();
            return Ok(coches);
        }
        [HttpGet]
        [Route("[action]")]

        [ProducesResponseType(typeof(IList<CocheParaComprarDTO>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult> GetCoche_Para_ComprarDTO(string? filtroColor, string? modelo)
        {
            var coches = await _context.Cars.Include(coche=>coche.Model)
                .Where(c=> c.Color.Contains(filtroColor) || (filtroColor==null) && (c.Model.Name.Contains(modelo) || modelo == null)
            )
                .Select(c=>new CocheParaComprarDTO(c.Id, c.Model.Name,
                c.Color, c.FuelType, c.Manufacturer, c.PurchasePrice)).ToListAsync();
            return Ok(coches);
        }
    }
}