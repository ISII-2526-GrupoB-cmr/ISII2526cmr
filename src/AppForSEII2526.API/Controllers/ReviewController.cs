
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
                _logger.LogError("Error: La tabla Review no existe");
                return NotFound();
            }

            var review = await _context.Reviews
              .Where(p => p.Id == id)
              .Include(p => p.ApplicationUser)
              .Include(p => p.ReviewItems) //join table PurchaseItems
                 .ThenInclude(pi => pi.Car) //then join table Cars
                     .ThenInclude(car => car.Model) //then join table Model
          .Select(p => new ReviewDetailDTO (
                            p.Id,
                            p.Country,
                            p.Created.Date,
                            p.ApplicationUser.UserName,
                            p.Drivertype,
                            p.ReviewItems.Select(pi => new ReviewItemDTO(
                               
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
                _logger.LogError($"Error: review con id {id} no existe");
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

            if (string.IsNullOrWhiteSpace(reviewForCreate.Country))
            {
                ModelState.AddModelError("Country", "Error! Pais de residencia no puede estar vacio");
            }

            if (!Enum.IsDefined(typeof(DriverType), reviewForCreate.Drivertype))
            {
                ModelState.AddModelError("Drivertype", "Error! DriverType debe ser 'novato' o 'experto'.");
            }

           
            if (reviewForCreate.Reviewitems == null || !reviewForCreate.Reviewitems.Any())
            {
                ModelState.AddModelError("Reviewitems", "Error! Ningun coche seleccionado para review");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

           
           
            if (user == null)
            {
                ModelState.AddModelError("Username", "Error! Tu nombre de usuario no esta registrado");
                return BadRequest(new ValidationProblemDetails(ModelState));
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
                    ModelState.AddModelError("ReviewItems", "Error! El coche seleccionado no existe");

                }
                else
                {
                    if (item.Description != null && !item.Description.StartsWith("Reseña para"))
                    {
                        ModelState.AddModelError("Description", "Error! La reseña debe empezar por Reseña para");

                    }
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
                ModelState.AddModelError("Purchase", $"Error! Ha habido un error, tu review no se ha guardado correctamente");
                return Conflict("Error" + ex.Message);

            }
            var reviewDetail = new ReviewDetailDTO(
                                review.Id,
                              review.Country,
                              review.Created.Date,
                              review.ApplicationUser.UserName,
                              review.Drivertype,
                              reviewForCreate.Reviewitems
                            );

            _logger.LogInformation($"Review with id {review.Id} creada con �xito");
            return CreatedAtAction("GetReview", new { id = review.Id }, reviewDetail);





        }


    }


}

