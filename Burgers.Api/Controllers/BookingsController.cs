using Burgers.Api.DTOs;
using Burgers.Core.Interfaces;
using Burgers.Models;
using Microsoft.AspNetCore.Mvc;
namespace Burgers.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
       

        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingservice)
        {
            _bookingService = bookingservice;
        }
        [HttpPost]
        public ActionResult<BookingResponseDto> PostBooking(CreateBookingDto dto)
        {
            
            var entity = new Booking
            {
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone,
                CustomerEmail = dto.CustomerEmail,
                BookingDate = dto.BookingDate,
                NumberOfGuests = dto.NumberOfGuests,
                SpecialRequests = dto.SpecialRequests,
                Address = dto.Address,
            };
           var res = _bookingService.CreateBooking(entity);
            return Ok(new BookingResponseDto { Address = res.Address, BookingDate = res.BookingDate, Id = res.Id, Status = res.Status});
        }
    }
}
