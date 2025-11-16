
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;
using System.Linq;


namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
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
              .Include(p => p.Applicationuser)
              .Include(p => p.ReviewItems) //join table PurchaseItems
                 .ThenInclude(pi => pi.Car) //then join table Cars
                     .ThenInclude(car => car.Model) //then join table Model
          .Select(p => new ReviewDetailDTO(
                            p.Id,
                            p.Country,
                            p.Created,
                            p.Applicationuser.UserName,
                            p.Drivertype,
                            p.ReviewItems.Select(pi => new ReviewItemDTO(
                                pi.Car.Id,
                                pi.Car.Model.Name,    // model
                                pi.Car.FuelType,      // fueltype
                                pi.Car.Manufacturer,
                                pi.Car.Color,
                                pi.Rating,
                                pi.Description
                            )).ToList<ReviewItemDTO>()
                        )).FirstOrDefaultAsync();

            if (review == null)
            {
                _logger.LogError($"Error: review with id {id} does not exist");
                return NotFound();
            }


            return Ok(review);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReview(ReviewForCreateDTO reviewForCreate)
        {



            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == reviewForCreate.Username);


            Review review = new Review(reviewForCreate.Country, DateTime.Today, user, reviewForCreate.Drivertype, new List<ReviewItem>()
                );

            if (reviewForCreate.Country == null)
            {
                ModelState.AddModelError("Country", "Error! Country is null");
            }

            if (user == null)
            {
                ModelState.AddModelError("Username", "Error! User does not exist");
            }


            if (reviewForCreate.Drivertype == null)
            {
                ModelState.AddModelError("Username", "Error! DriverType is null");
            }


            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var reviewcar = reviewForCreate.Reviewitems.Select(pi => pi.Model).ToList();

            var cars = _context.Cars
                .Include(c => c.Model)
                .Where(c => reviewcar.Contains(c.Model.Name))
                .Select(c => new { c.Id, c.Model.Name })
                .ToList();

            foreach (var item in reviewForCreate.Reviewitems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Model);

                if (car == null)
                {
                    ModelState.AddModelError("Car", $"Error! Coche con modelo {item.Model} no existe");

                }
                else
                {
                    review.ReviewItems.Add(new ReviewItem(car.Id, item.Description, item.Rating, review));
                }


            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(review);

            try
            {
                //we store in the database both purchase and its purchaseItems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Purchase", $"Error! There was an error while saving your review, plese, try again later");
                return Conflict("Error" + ex.Message);

            }
            var reviewDetail = new ReviewDetailDTO(
                              review.Id,
                              review.Country,
                              review.Created,
                              review.Applicationuser.UserName,
                              review.Drivertype,
                              reviewForCreate.Reviewitems
                            );

            return CreatedAtAction("GetReview", new { id = review.Id }, reviewDetail);





        }


    }


}

