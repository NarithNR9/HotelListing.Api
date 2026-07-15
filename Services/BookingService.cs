using System.IdentityModel.Tokens.Jwt;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Data.Enums;
using HotelListing.Api.DTOs.Booking;
using HotelListing.Api.Results;

namespace HotelListing.Api.Services;

public class BookingService(HotelListingDbContext context, IHttpContextAccessor httpContextAccessor) : IBookingService
{
    public async Task<Result<IEnumerable<GetBookingDto>>> GetBookingForHotelAsync(int hotelId)
    {
        var hotelsExist = await context.Hotels.AnyAsync(h => h.Id == hotelId);
        if (!hotelsExist)
        {
            return Result<IEnumerable<GetBookingDto>>.Failure(new Error(ErrorCodes.NotFound, "Hotel not found"));
        }

        var bookings = await context.Bookings
            .Where(b => b.HotelId == hotelId)
            .OrderBy(b => b.CheckInDate)
            .Select(b => new GetBookingDto(
                b.Id,
                b.HotelId,
                b.Hotel!.Name,
                b.CheckInDate,
                b.CheckOutDate,
                b.Guests,
                b.TotalPrice,
                b.Status.ToString(),
                b.CreatedAt,
                b.UpdatedAt
            ))
            .ToListAsync();
        return Result<IEnumerable<GetBookingDto>>.Success(bookings);
    }

    public async Task<Result<GetBookingDto>> CreateBookingAsync(CreateBookingDto dto)
    {
        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Result<GetBookingDto>.Failure(new Error(ErrorCodes.Validation, "User is required"));
        }

        var nights = dto.CheckIn.DayNumber - dto.CheckOut.DayNumber;
        if (nights < 0)
        {
            return Result<GetBookingDto>.Failure(new Error(ErrorCodes.Validation, "Check-out date must be after check-in date"));
        }

        if (dto.Guests <= 0)
        {
            return Result<GetBookingDto>.Failure(new Error(ErrorCodes.Validation, "Guests must be greater than 0"));
        }

        var hotel = await context.Hotels.FindAsync(dto.HotelId);
        if (hotel == null)
        {
            return Result<GetBookingDto>.Failure(new Error(ErrorCodes.NotFound, "Hotel not found"));
        }

        var overlaps = await context.Bookings.AnyAsync(b => b.HotelId == dto.HotelId
                    && b.Status != BookingStatus.Cancelled
                    && b.CheckInDate < dto.CheckOut
                    && b.CheckOutDate > dto.CheckIn
                    && b.UserId != userId);
        if (overlaps)
        {
            return Result<GetBookingDto>.Failure(new Error(ErrorCodes.Validation, "Hotel is already booked for the selected dates"));
        }

        var totalPrice = hotel.PerNightRate * nights;

        var booking = new Booking
        {
            HotelId = dto.HotelId,
            UserId = userId,
            CheckInDate = dto.CheckIn,
            CheckOutDate = dto.CheckOut,
            Guests = dto.Guests,
            TotalPrice = totalPrice,
            Status = BookingStatus.Pending,
        };

        context.Bookings.Add(booking);
        await context.SaveChangesAsync();

        var created = new GetBookingDto(
            booking.Id,
            hotel.Id,
            hotel.Name,
            dto.CheckIn,
            dto.CheckOut,
            dto.Guests,
            totalPrice,
            BookingStatus.Pending.ToString(),
            booking.CreatedAt,
            booking.UpdatedAt
        );

        return Result<GetBookingDto>.Success(created);
    }
}
