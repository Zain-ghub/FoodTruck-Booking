using Burgers.Data;
using Burgers.Models;
using Burgers.Core.Interfaces;


namespace Burgers.Services
{
    public class BookingService : IBookingService
    {
        private readonly BurgersContext _context;

        public BookingService(BurgersContext context)
        {
            _context = context;
        }
        public bool UpdateStatus(int id, string status)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return false;
            }
            else
            {
                booking.Status = status;
                _context.SaveChanges();
                return true;
            }
        }
        public List<Booking> GetBookings()
        {
            return _context.Bookings.OrderBy(b => b.BookingDate).ToList();
        }
        public Booking CreateBooking(Booking booking)
        {
            booking.Status = "Pending";
            _context.Bookings.Add(booking);
            _context.SaveChanges();
            return booking;
        }
    }
}
