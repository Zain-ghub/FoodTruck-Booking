using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Burgers.Models;
using Burgers.Services;

namespace Burgers.Pages
{
    public class BookingsModel : PageModel
    {
        private readonly IBookingService _bookingService;

        public BookingsModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public IActionResult OnPostConfirm(int id)
        {
           var result = _bookingService.UpdateStatus(id, "Confirmed");
            return result ? RedirectToPage() : NotFound();
            
        }
        public IActionResult OnPostDecline(int id)
        {
            var result = _bookingService.UpdateStatus(id, "Declined");
            return result ? RedirectToPage() : NotFound();
        }
        public List<Booking> Bookings  { get; set; } = new();
        public void OnGet()
        {
            Bookings = _bookingService.GetBookings();
        }
    }
}
