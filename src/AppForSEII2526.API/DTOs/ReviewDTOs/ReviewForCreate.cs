using AppForSEII2526.API.Models;
using System.Drawing;
using System.Net;

namespace AppForSEII2526.API.DTOs.ReviewDTOs
{
    public class ReviewForCreate
    {
        public ReviewForCreate(string country, string driverType, string username, IList<ReviewItemDTO> reviewitems, string manufacturer, string color, int rating, string description, string model) {


            Country = country ?? throw new ArgumentNullException(nameof(name));
            DriverType = driverType ?? throw new ArgumentNullException(nameof(surname));
            Username = username ?? throw new ArgumentNullException(nameof(address));
            Reviewitems= reviewitems ?? throw new ArgumentNullException(nameof(rentalItems));
            Manufacturer = manufacturer;
            Color= color;
            Rating= rating;
            Description= description ?? throw new ArgumentException(nameof(description));
            Model= model;
        }

    }
    }
}
