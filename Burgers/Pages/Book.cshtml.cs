
using Burgers.Models;
using Burgers.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Burgers.Pages
{
    public class BookModel : PageModel
    {
        
        private readonly IBookingService _bookingService;

        public BookModel(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
        [BindProperty]
        public Booking Booking { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (Booking.BookingDate < DateTime.Now)
            {
                ModelState.AddModelError("Booking.BookingDate", "Booking date cannot be in the past.");
            }
            if (ModelState.IsValid)
            {
                
                _bookingService.CreateBooking(Booking);
                return RedirectToPage("Confirmation");
            }
            return Page();
        }
    }
}
