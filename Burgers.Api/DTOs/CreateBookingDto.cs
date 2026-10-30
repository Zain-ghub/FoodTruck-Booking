using System.ComponentModel.DataAnnotations;
using Burgers.Core;
namespace Burgers.Api.DTOs
{
    public class CreateBookingDto
    {
        [Required]
        public required string CustomerName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]

        public string? CustomerEmail { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        [Required]
        public required string CustomerPhone { get; set; }
        [FutureDate(ErrorMessage = "Booking date must be in the future")]
        public DateTime BookingDate { get; set; }
        [Range(1, 700, ErrorMessage = "Number of guests must be at least 1 and at most 700")]
       
        public int? NumberOfGuests { get; set; }
        
        public string? SpecialRequests { get; set; }

        [Required]
        public required string Address { get; set; }
    }
}
