using Burgers.Data;
using Burgers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;

namespace Burgers.Pages
{
    public class ConfirmationModel : PageModel
    {
        private readonly BurgersContext _context;

        public ConfirmationModel(BurgersContext context)
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
        public void OnGet()
        {
        }
    }
}
