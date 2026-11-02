using Burgers.Api.DTOs;
using Burgers.Core.Interfaces;
using Burgers.Models;
using Microsoft.AspNetCore.Authorization;
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
            return Ok(new BookingResponseDto { Address = res.Address, BookingDate = res.BookingDate, Id = res.Id, Status = res.Status });
        }
        //[Authorize]
        [HttpGet]
        public ActionResult<List<BookingAdminDto>> GetAllBookings()
        {
            var ent = _bookingService.GetBookings();

            var result = ent.Select( b => new BookingAdminDto {
                    Address = b.Address,
                    BookingDate = b.BookingDate,
                    Id = b.Id,
                    Status = b.Status,
                    SpecialRequests = b.SpecialRequests,
                    CustomerName = b.CustomerName,
                    CustomerPhone = b.CustomerPhone,
                    CustomerEmail = b.CustomerEmail,
                    NumberOfGuests = b.NumberOfGuests,
            }).ToList();

            return Ok(result);
        }

        [HttpPost("{id}/confirm")]
        public ActionResult ConfirmBooking(int id)
        {
            var res = _bookingService.UpdateStatus(id, "Confirmed");
            return res ? NoContent() : NotFound();
        }

        [HttpPost("{id}/decline")]
        public ActionResult DeclineBooking(int id)
        {
           var  res = _bookingService.UpdateStatus(id, "Declined");
            return res ? NoContent() : NotFound();
        }

    }

}
