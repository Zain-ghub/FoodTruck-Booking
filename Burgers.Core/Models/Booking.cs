using System.ComponentModel.DataAnnotations;
namespace Burgers.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Display(Name = "Your Name")]
        public required string CustomerName { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Your Email")]
        public string? CustomerEmail { get; set; }
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Your Phone")]  
        public required string CustomerPhone { get; set; }
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; }
        [Range(1, 690, ErrorMessage = "Number of guests must be at least 1")]
        [Display(Name = "Number of Guests")]
        public int? NumberOfGuests { get; set; }
        [Display(Name = "Special Requests")]
        public string? SpecialRequests { get; set; }
        public string? Status { get; set; }
        [Display(Name = "Event Address")]
        public required string Address { get; set; }
    }
}
