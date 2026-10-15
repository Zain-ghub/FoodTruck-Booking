using Burgers.Data;
using Burgers.Models;
using Burgers.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Burgers.Tests
{
    public class UnitTest1
    {
        private BurgersContext SetupInMemoryDatabase()
        {
            var options = new DbContextOptionsBuilder<BurgersContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            

            return new BurgersContext(options);
        }
        [Fact]
        public void CreateBooking_NewBooking_SetsStatusToPending()
        {
            //arrange
            var context = SetupInMemoryDatabase();
            var bookingservice = new BookingService(context);
            
            bookingservice.CreateBooking(new Booking { CustomerName = "John", CustomerPhone = "43262362", Address = "booking street" });
            //act with assert because simple test
            context.Bookings.First().Status.Should().Be("Pending");
        }
        [Fact]
        public void GetBookings_BookingsOutOfOrder_ReturnsOrderedByDate()
        {
            var context = SetupInMemoryDatabase();
            var bookingservice = new BookingService(context);
            bookingservice.CreateBooking(new Booking { CustomerName = "John", CustomerPhone = "43262362", Address = "booking street", BookingDate = new DateTime(2026,12,1) });
            bookingservice.CreateBooking(new Booking { CustomerName = "Early John", CustomerPhone = "43262362", Address = " early booking street", BookingDate =new DateTime(2026,11,1) });
            //
            var result = bookingservice.GetBookings();
            result.Should().BeInAscendingOrder(b=>b.BookingDate);
            result.Should().HaveCount(2);
        }
        [Fact]
        public void UpdateStatus_UnknownId_ReturnsFalse()
        {
            var context = SetupInMemoryDatabase();
            var bookingservice = new BookingService(context);
            //
            var result = bookingservice.UpdateStatus(999, "Confirmed");
            result.Should().BeFalse();
        }
        [Fact]
        public void UpdateStatus_ExistingBooking_UpdatesStatusAndReturnsTrue()
        {
            var context = SetupInMemoryDatabase();
            var bookingservice = new BookingService(context);
            var booking = bookingservice.CreateBooking(new Booking { CustomerName = "John", CustomerPhone = "43262362", Address = "booking street", BookingDate = new DateTime(2026, 12, 1) });

            var result = bookingservice.UpdateStatus(booking.Id, "Confirmed");
            result.Should().BeTrue();
            context.Bookings.First().Status.Should().Be("Confirmed");
        }

    }
}