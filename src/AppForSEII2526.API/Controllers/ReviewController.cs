
using AppForSEII2526.API.DTOs.ReviewDTOs;


namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController:ControllerBase
    {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<ReviewController> _logger;

            public ReviewController(ApplicationDbContext context, ILogger<ReviewController> logger)
            {
                _context = context;
                _logger = logger;
            }

            [HttpGet]
            [Route("[action]")]
            [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public async Task<ActionResult> GetReview(int id)
            {
                if (_context.Reviews == null)
                {
                    _logger.LogError("Error: Review table does not exist");
                    return NotFound();
                }

            var review = await _context.Reviews
              .Where(p => p.Id == id)
              .Include(p => p.ReviewItems) //join table PurchaseItems
                 .ThenInclude(pi => pi.Car) //then join table Cars
                     .ThenInclude(car => car.Model) //then join table Model
          .Select(p => new ReviewDetailDTO(
                            p.Id,
                            p.country,
                            p.created,
                            p.ApplicationUser.UserName,
                            (DriverType)p.drivertype,
                            p.ReviewItems.Select(pi => new ReviewItemDTO(
                                pi.Car.Id,
                                pi.Car.Model.Name,    // model
                                pi.Car.FuelType,      // fueltype
                                pi.Car.Manufacturer,
                                pi.Car.Color,
                                pi.Rating,
                                pi.Description
                            )).ToList()
                        )).FirstOrDefaultAsync();

            if (review == null)
                {
                    _logger.LogError($"Error: review with id {id} does not exist");
                    return NotFound();
                }


                return Ok(review);
            }

           
    }
}
