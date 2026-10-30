namespace Burgers.Api.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }
        public required string Status { get; set; }

        public DateTime BookingDate { get; set; }
        public required string Address { get; set; }
    }

}
