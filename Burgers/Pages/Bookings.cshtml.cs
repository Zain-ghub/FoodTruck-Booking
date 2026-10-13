using Microsoft.AspNetCore.Mvc;
using Burgers.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Burgers.Models;

namespace Burgers.Pages
{
    public class BookingsModel : PageModel
    {
        private readonly BurgersContext _context;

        public BookingsModel(BurgersContext context)
        {
            _context = context;
        }
        private IActionResult UpdateStatus(int id, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            else
            {
                booking.Status = status;
                _context.SaveChanges();
                return RedirectToPage();
            }
        }
        public IActionResult OnPostConfirm(int id)
        {
            return UpdateStatus(id, "Confirmed");
        }
        public IActionResult OnPostDecline(int id)
        {
            return UpdateStatus(id, "Declined");
        }
        public List<Booking> Bookings  { get; set; } = new();
        public void OnGet()
        {
            Bookings = _context.Bookings.OrderBy(b => b.BookingDate).ToList();
        }
    }
}
