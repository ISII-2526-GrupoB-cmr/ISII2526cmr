
using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.DTOs.ReviewDTOs;
using AppForSEII2526.API.Models;


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
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReview(ReviewForCreateDTO reviewForCreate) 
        {




            Review review = new Review(reviewForCreate.Country, reviewForCreate.Username,
                reviewForCreate.Manufacturer, reviewForCreate.Color, reviewForCreate.Rating, reviewForCreate.Description, reviewForCreate.Model,
                reviewForCreate.Drivertype, new List<ReviewItem>(), reviewForCreate.Fueltype
                );

            if(reviewForCreate.Country == null)
            {
                ModelState.AddModelError("Country", "Error! Country is null");
            }
            if (reviewForCreate.Rating < 1 || reviewForCreate.Rating > 5)
            {
                ModelState.AddModelError("Rating", "Error! Rating must be between 1 and 5");
            }

            var user = _context.ApplicationUsers.FirstOrDefault(au => au.UserName == reviewForCreate.Username);
            if (reviewForCreate.Username == null)
            {
                ModelState.AddModelError("Username", "Error! Username is null");
            }
            if (reviewForCreate.Drivertype == null) {
                ModelState.AddModelError("Username", "Error! DriverType is null");
            }
            if (reviewForCreate.Model == null) {
                ModelState.AddModelError("Username", "Error! model is null");
            }
            if (reviewForCreate.Manufacturer == null)
            {
                ModelState.AddModelError("Username", "Error! manufacturer is null");
            }
            if (reviewForCreate.Color == null)
            {
                ModelState.AddModelError("Username", "Error! color is null");
            }
            if (reviewForCreate.Fueltype== null) {
                ModelState.AddModelError("Username", "Error! model is null");
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
                ModelState.AddModelError("Purchase", $"Error! There was an error while saving your purchase, plese, try again later");
                return Conflict("Error" + ex.Message);

            }
            var reviewDetail = new ReviewDetailDTO(
                              review.Id,
                              review.country,
                              review.created,
                              review.ApplicationUser.UserName,
                              review.drivertype,
                              reviewForCreate.Reviewitems
                            );

            return CreatedAtAction("GetRental", new { id = review.Id }, reviewDetail);





        }


    }

 
}
