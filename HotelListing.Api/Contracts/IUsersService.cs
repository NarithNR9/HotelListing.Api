using HotelListing.Api.Common.Results;
using HotelListing.Api.DTOs.Auth;

namespace HotelListing.Api.Contracts;

public interface IUsersService
{
    string UserId { get; }
    Task<Result<string>> LoginUserAsync(LoginUserDto dto);
    Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
}