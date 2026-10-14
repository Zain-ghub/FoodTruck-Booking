using Burgers.Models;

namespace Burgers.Services
{
    public interface IBookingService
    {
        bool UpdateStatus(int id, string status);
        List<Booking> GetBookings();
        Booking CreateBooking(Booking booking);
    }
}
