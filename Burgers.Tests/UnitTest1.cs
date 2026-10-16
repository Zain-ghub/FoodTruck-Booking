using Burgers.Data;
using Burgers.Models;
using Burgers.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Filters;
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
            bookingservice.CreateBooking(new Booking { CustomerName = "John", CustomerPhone = "43262362", Address = "booking street", BookingDate = new DateTime(2026, 12, 1) });
            bookingservice.CreateBooking(new Booking { CustomerName = "Early John", CustomerPhone = "43262362", Address = " early booking street", BookingDate = new DateTime(2026, 11, 1) });
            //
            var result = bookingservice.GetBookings();
            result.Should().BeInAscendingOrder(b => b.BookingDate);
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
        [Fact]
        public void GetMenuItems_UnavailableItem_IsHidden()
        {
            var context = SetupInMemoryDatabase();
            var menuservice = new MenuService(context);

            menuservice.AddMenuItem(new MenuItem { Name = "Hidden John Burger", Price = 300, Category = "Food", IsAvailable = false });
            menuservice.AddMenuItem(new MenuItem { Name = "Available John Burger", Price = 250, Category = "Food", IsAvailable = true });

            var result = menuservice.GetMenuItems();
            result.Should().ContainSingle().Which.Name.Should().Be("Available John Burger");
        }

        [Fact]
        public void GetMenuItems_ItemsOutOfOrder_ReturnsOrderedByCategoryThenName()
        {
            var context = SetupInMemoryDatabase();
            var menuservice = new MenuService(context);

            menuservice.AddMenuItem(new MenuItem { Name = "Michael Burger", Price = 300, Category = "Food", IsAvailable = true });
            menuservice.AddMenuItem(new MenuItem { Name = "Lemonade", Price = 50, Category = "Drink", IsAvailable = true });
            menuservice.AddMenuItem(new MenuItem { Name = "John Burger", Price = 250, Category = "Food", IsAvailable = true });

            var result = menuservice.GetMenuItems();

            result.Select(m => m.Name).Should().Equal("Lemonade", "John Burger", "Michael Burger");
        }
        [Fact]
        public void GetAllMenuItems_UnavailableItem_IsIncluded()
        {
            var context = SetupInMemoryDatabase();
            var menuservice = new MenuService(context);

            menuservice.AddMenuItem(new MenuItem { Name = "Lemonade", Price = 50, Category = "Drink", IsAvailable = false });
            menuservice.AddMenuItem(new MenuItem { Name = "John Burger", Price = 250, Category = "Food", IsAvailable = true });

            var result = menuservice.GetAllMenuItems();
            result.Should().HaveCount(2);
            result.Should().Contain(m => !m.IsAvailable);
        }

        [Fact]
        public void UpdateMenuItem_UnknownId_ReturnsFalse()
        {
            var context = SetupInMemoryDatabase();
            var menuservice = new MenuService(context);

            var result = menuservice.UpdateMenuItem(new MenuItem { Id = 999 ,Name = "John Burger", Price = 250, Category = "Food", IsAvailable = true });
            result.Should().BeFalse();
            context.MenuItems.Should().BeEmpty();
        }

        [Fact]
        public void DeleteMenuItem_UnknownId_ReturnsFalse()
        {
            var context = SetupInMemoryDatabase();
            var menuservice = new MenuService(context);

            var result = menuservice.DeleteMenuItem(999);
            result.Should().BeFalse();
        }
    }
}