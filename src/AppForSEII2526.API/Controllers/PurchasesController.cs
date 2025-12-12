using AppForSEII2526.API.DTOs.PurchaseDTOs;
using AppForSEII2526.API.DTOs.RentalDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PurchasesController> _logger;

        public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
        {
            _context = context;
            _logger = logger;
            //uso del logger para registra la inicializacion del controlador
            _logger.LogInformation("TodoService initialized");
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetPurchase(int id)
        {
            if (_context.Purchases == null)
            {
                _logger.LogError("Error: Purchases table does not exist");
                return NotFound();
            }

            var purchase = await _context.Purchases
                 .Where(p => p.Id == id)
                 .Include(p => p.PurchaseItems) //join table PurchaseItems
                    .ThenInclude(pi => pi.Car) //then join table Cars
                        .ThenInclude(car => car.Model) //then join table Model
             .Select(p => new PurchaseDetailDTO(p.Id, p.PurchasingDate, p.ApplicationUser.Name,
                    p.ApplicationUser.Surname, p.ApplicationUser.Address,
                    p.PurchasingPrice,
                    p.PurchaseItems
                        .Select(pi => new PurchaseItemDTO(
                            pi.Car.Id,
                            pi.Car.PurchasePrice,
                            pi.Car.Model.Name,
                            pi.Car.Description,
                            pi.Quantity,
                            pi.Car.Color
                            
                        )).ToList<PurchaseItemDTO>())).FirstOrDefaultAsync();


            if (purchase == null)
            {
                _logger.LogError($"Error: Purchase with id {id} does not exist");
                return NotFound();
            }


            return Ok(purchase);
        }

        

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(PurchaseDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            //any validation defined in PurchaseForCreate is checked before running the method so they don't have to be checked again
            if (purchaseForCreate.PurchaseItems == null || purchaseForCreate.PurchaseItems.Count == 0)
                ModelState.AddModelError("PurchaseItems", "Error! Ningun coche seleccionado");

            // if (!_context.ApplicationUsers.Any(au=>au.UserName==rentalForCreate.CustomerUserName))
            var user = _context.ApplicationUsers.FirstOrDefault(au => au.Name == purchaseForCreate.CustomerName &&
                                                                au.Surname == purchaseForCreate.CustomerSurname);
            if (user == null)
                ModelState.AddModelError("PurchaseApplicationUser", "Error! Tu nombre no esta registrado");

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
            var purchaseNames = purchaseForCreate.PurchaseItems.Select(pi => pi.Modelo).ToList<string>();

            var cars = _context.Cars
                .Include(c => c.PurchaseItems)
                .ThenInclude(pi => pi.Purchase)
                .Where(c => purchaseNames.Contains(c.Model.Name))
                .Select(c => new
                {
                    c.Id,
                    c.Model.Name,
                    c.QuantityForPurchase,
                    c.PurchasePrice,
                    NumberOfPurchaseItems = c.PurchaseItems.Sum(pi => pi.Quantity)
                })
                .ToList();


            Purchase purchase = new Purchase(purchaseForCreate.CustomerName, purchaseForCreate.CustomerSurname,
                purchaseForCreate.DeliveryAddress, purchaseForCreate.PaymentMethod,
                new List<PurchaseItem>());


            purchase.ApplicationUser = user;


            
            purchase.PurchasingPrice = 0;

            foreach (var item in purchaseForCreate.PurchaseItems)
            {
                var car = cars.FirstOrDefault(c => c.Name == item.Modelo);


                if (car == null)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! Car Model '{item.Modelo}' is not available for being purchased");
                }
                if (car.QuantityForPurchase == 0)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! There are no more cars with model '{item.Modelo}' available");
                }
                if (car.QuantityForPurchase < item.Quantity && car.QuantityForPurchase != 0)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! There are only '{car.QuantityForPurchase}' available for model'{item.Modelo}");
                }
                if (item.Quantity == 0)
                {
                    ModelState.AddModelError("PurchaseItems", $"Error! You cannot select quantity 0 for purchase");
                }
                else
                {
  
                    purchase.PurchasingPrice += car.PurchasePrice * item.Quantity;
                    // purchase does not exist in the database yet and does not have a valid Id, so we must relate purchaseitem to the object purchase
                    purchase.PurchaseItems.Add(new PurchaseItem(car.Id, item.Quantity, purchase, car.PurchasePrice, purchase.PurchasingPrice, item.CarColor, item.Description));
                }
                
            }

            

            
          


            //if there is any problem because of the available quantity of movies or because the movie does not exist
            if (ModelState.ErrorCount > 0)
            {
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            _context.Add(purchase);

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

            //it returns purchaseDetail
            var purchaseDetail = new PurchaseDetailDTO(purchase.Id, purchase.PurchasingDate,
                purchase.ApplicationUser.Name, purchase.ApplicationUser.Surname,
                purchase.ApplicationUser.Address, purchase.PurchasingPrice,
                purchaseForCreate.PurchaseItems);

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseDetail);
        }
        
    }

       


}