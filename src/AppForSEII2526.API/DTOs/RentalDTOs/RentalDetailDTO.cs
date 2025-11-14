namespace AppForSEII2526.API.DTOs.RentalDTOs
{
    public class RentalDetailDTO : RentalForCreateDTO
    {
        public RentalDetailDTO(int id, DateTime rentalDate, string name, string surname,
            string address, PaymentMethodTypes paymentMethod, DateTime startDate,
            DateTime endDate, IList<RentalItemDTO> rentalItems)
            
        {
            Name =name ?? throw new ArgumentNullException(nameof(name));
            Surname = surname ?? throw new ArgumentNullException(nameof(surname));
            Address = address ?? throw new ArgumentNullException(nameof(address));
            PaymentMethod = paymentMethod;
            StartDate = startDate;
            EndDate = endDate;
            RentalItems = rentalItems ?? throw new ArgumentNullException(nameof(rentalItems));
            Id = id;
            RentalDate = rentalDate;
        }
        public int Id { get; set; }

        public DateTime RentalDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is not RentalDetailDTO other)
                return false;

            return Id == other.Id &&
                   Name == other.Name &&
                   Surname == other.Surname &&
                   Address == other.Address &&
                   PaymentMethod == other.PaymentMethod &&
                   StartDate == other.StartDate &&
                   EndDate == other.EndDate &&
                   TotalPrice == other.TotalPrice &&
                   RentalItems.SequenceEqual(other.RentalItems);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Surname, Address, PaymentMethod, StartDate, EndDate);
        }
    }
}