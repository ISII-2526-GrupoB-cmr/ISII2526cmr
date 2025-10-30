using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                if (_context.Rentals == null)
                {
                    _logger.LogError("Error: Review table does not exist");
                    return NotFound();
                }

                var review = await _context.Reviews
                 .Where(r => r.Id == id)
                     .Include(r => r.ReviewItems) //join table RentalItems
                        .ThenInclude(ri => ri.Car) //then join table Movies
                            .ThenInclude(movie => movie.Model) //then join table Genre
                 .Select(r => new ReviewDetailDTO(r.model, r.Manufacturer, r.color,
                        r.rating, r.description, r.name, r.country, r.DriverType, r.created)
                            .Select(ri => new ReviewDTO(ri.Movie.Id,
                                    ri.Movie.Title, ri.Movie.Genre.Name,
                                    ri.Movie.PriceForRenting, ri.Description)).ToList<ReviewDTO>()))
                 .FirstOrDefaultAsync();


                if (review == null)
                {
                    _logger.LogError($"Error: review with id {id} does not exist");
                    return NotFound();
                }


                return Ok(review);
            }

           
    }
}
