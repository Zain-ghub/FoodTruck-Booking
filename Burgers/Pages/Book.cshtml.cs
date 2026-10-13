using Burgers.Data;
using Burgers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Burgers.Pages
{
    public class BookModel : PageModel
    {
        private readonly BurgersContext _context;

        public BookModel(BurgersContext context)
        {
            _context = context;
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
                Booking.Status = "Pending"; // Set the default status to "Pending"

                _context.Bookings.Add(Booking);
                _context.SaveChanges();
                // Redirect to a confirmation page or display a success message
                return RedirectToPage("Confirmation");
            }
            return Page();

        }
    }
}
