using HotelListing.Api.Contracts;
using HotelListing.Api.DTOs.Booking;
using HotelListing.Api.DTOs.Hotel;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelBookingsController(IBookingService bookingService) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetBookingDto>>> GetBookings([FromRoute] int hotelId)
    {
        var bookings = await bookingService.GetBookingForHotelAsync(hotelId);
        return ToActionResult(bookings);
    }

    [HttpPost]
    public async Task<ActionResult<GetBookingDto>> CreateBooking([FromBody] CreateBookingDto bookingDto)
    {
        var booking = await bookingService.CreateBookingAsync(bookingDto);
        return ToActionResult(booking);
    }
}
