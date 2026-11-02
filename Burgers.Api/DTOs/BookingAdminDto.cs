using System.ComponentModel.DataAnnotations;

namespace Burgers.Api.DTOs
{
    public class BookingAdminDto
        {
            public int Id { get; set; }          
            public required string CustomerName { get; set; }
            [EmailAddress(ErrorMessage = "Invalid email address")]        
            public string? CustomerEmail { get; set; }
            [Phone(ErrorMessage = "Invalid phone number")]       
            public required string CustomerPhone { get; set; }         
            public DateTime BookingDate { get; set; }
            [Range(1, 700, ErrorMessage = "Number of guests must be at least 1 or at most 700")]         
            public int? NumberOfGuests { get; set; }         
            public string? SpecialRequests { get; set; }
            public string? Status { get; set; }   
            public required string Address { get; set; }
        }
 }

