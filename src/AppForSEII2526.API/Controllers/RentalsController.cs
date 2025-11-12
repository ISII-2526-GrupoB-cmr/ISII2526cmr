using AppForSEII2526.API.DTOs.RentalDTOs;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RentalsController> _logger;

        public RentalsController(ApplicationDbContext context, ILogger<RentalsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            if (_context.Rentals == null)
            {
                _logger.LogError("Error: Rentals table does not exist");
                return NotFound();
            }

            var rental = await _context.Rentals
             .Where(r => r.Id == id)
                 .Include(r => r.RentalItems) //join table RentalItems
                    .ThenInclude(ri => ri.Car) //then join table Movies
                        .ThenInclude(car => car.Model) //then join table Genre
             .Select(r => new RentalDetailDTO(r.Id, r.RentignDate, r.ApplicationUser.Name,
                    r.ApplicationUser.Surname, r.ApplicationUser.Address,
                    (PaymentMethodTypes)r.PaymentMethod,
                    DateTime.SpecifyKind(r.StartDate, DateTimeKind.Local),
                    DateTime.SpecifyKind(r.EndDate, DateTimeKind.Local),
                    r.RentalItems
                        .Select(ri => new RentalItemDTO(ri.Car.Id,
                                 ri.Car.Model.Name, ri.Car.Manufacturer,
                                ri.Car.RentingPrice)).ToList<RentalItemDTO>()))
             .FirstOrDefaultAsync();


            if (rental == null)
            {
                _logger.LogError($"Error: Rental with id {id} does not exist");
                return NotFound();
            }


            return Ok(rental);
        
    
}
        
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RentalDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateRental([FromBody]RentalForCreateDTO rentalForCreate)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (rentalForCreate.RentalItems == null || rentalForCreate.RentalItems.Count==0)
                ModelState.AddModelError("RentalItems", "Error! Rental items list is null");
            
            if(rentalForCreate.Quantity<=0)
                ModelState.AddModelError("Quantity", "Error! You must select a quantity greater than 0");
            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.Name == rentalForCreate.Name && au.Surname == rentalForCreate.Surname);
            if (user == null)
                ModelState.AddModelError("RentalApplicationUser", "Error! UserName is not registered");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));


            var rentingnames = rentalForCreate.RentalItems.Select(ri => ri.Modelo).ToList<string>();

            var cars = _context.Cars
                .Include(c => c.Model)
                .Include(m => m.RentalItems)
                .ThenInclude(ri => ri.Rental)
                .Where(m => rentingnames.Contains(m.Model.Name))
                .ToList();


            Rental rental = new Rental(rentalForCreate.Name, rentalForCreate.Surname,
                                       rentalForCreate.Address, DateTime.Now,
                                       rentalForCreate.PaymentMethod,
                                       rentalForCreate.StartDate, rentalForCreate.EndDate, new List<RentalItem>());
            //we use an anonymous type https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types
            rental.DeliveryCarDealer = "Default Dealer"; // o el nombre del concesionario real




            rental.ApplicationUser = user;


            rental.TotalPrice = 0;
            var numDays = (rental.EndDate - rental.StartDate).TotalDays;


            foreach (var item in rentalForCreate.RentalItems)
            {
                var car = cars.FirstOrDefault(m => m.Model.Name == item.Modelo);
                //we must check that there is enough quantity to be rented in the database
                if (car == null) 
                {
                    ModelState.AddModelError("RentalItems", $"Error! Model car '{item.Modelo}' is not available for being rented from {rentalForCreate.StartDate.ToShortDateString()} to {rentalForCreate.EndDate.ToShortDateString()}");
                }

                var rentedCount = car.RentalItems.Count(ri =>
               ri.Rental.StartDate <= rentalForCreate.EndDate &&
               ri.Rental.EndDate >= rentalForCreate.StartDate);


                var disponible = car.QuantityForRenting - rentedCount;

                if(disponible< rentalForCreate.Quantity)
                {
                    ModelState.AddModelError("RentalItems", $"Error! Model car '{item.Modelo}' is not available for being rented from {rentalForCreate.StartDate.ToShortDateString()} to {rentalForCreate.EndDate.ToShortDateString()}");

                }
                // rental does not exist in the database yet and does not have a valid Id, so we must relate rentalitem to the object rental
                var rentalItem = new RentalItem
                {
                    CarId = car.Id,
                    Rental = rental,
                    Quantity = rentalForCreate.Quantity,
                    RentingPrice = car.RentingPrice,
                    Manufacturer = car.Manufacturer
                };

                rental.RentalItems.Add(rentalItem);
            }

            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            rental.TotalPrice = rental.RentalItems.Sum(ri => ri.RentingPrice * numDays);


            //if there is any problem because of the available quantity of movies or because the movie does not exist
           

            _context.Add(rental);

            try
            {
                //we store in the database both rental and its rentalitems
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException?.Message ?? "(sin inner exception)";
                _logger.LogError($"Error saving rental: {ex.Message} | Inner: {inner}");
                return Conflict($"Error: {ex.Message} | Inner: {inner}");
            }


            //it returns rentalDetail
            var rentalDetail = new RentalDetailDTO(
                rental.Id, 
                rental.RentignDate,
                rental.ApplicationUser.Name,
                rental.ApplicationUser.Surname,
                rental.ApplicationUser.Address, 
                rentalForCreate.PaymentMethod,
                rental.StartDate, 
                rental.EndDate,
                rentalForCreate.RentalItems);

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalDetail);
        }
    }

}