using Burgers.Api.DTOs;
using Burgers.Api.Hubs;
using Burgers.Core.Interfaces;
using Burgers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
namespace Burgers.Api.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {


        private readonly IBookingService _bookingService;
        private readonly IHubContext<BookingsHub> _hubContext;

        public BookingsController(IBookingService bookingservice, IHubContext<BookingsHub> hubContext)
        {
            _bookingService = bookingservice;
            _hubContext = hubContext;
        }
        [HttpPost]
        public async Task<ActionResult<BookingResponseDto>> PostBooking(CreateBookingDto dto)
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
            await _hubContext.Clients.All.SendAsync("bookingCreated", new {res.Id, res.CustomerName, res.BookingDate, res.Address });
            return Ok(new BookingResponseDto { Address = res.Address, BookingDate = res.BookingDate, Id = res.Id, Status = res.Status });
        }
        [Authorize]
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
        [Authorize]
        [HttpPost("{id}/confirm")]
        public ActionResult ConfirmBooking(int id)
        {
            var res = _bookingService.UpdateStatus(id, "Confirmed");
            return res ? NoContent() : NotFound();
        }
        [Authorize]
        [HttpPost("{id}/decline")]
        public ActionResult DeclineBooking(int id)
        {
           var  res = _bookingService.UpdateStatus(id, "Declined");
            return res ? NoContent() : NotFound();
        }

    }

}
