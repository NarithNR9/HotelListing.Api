using HotelListing.Api.DTOs.Auth;
using HotelListing.Api.Results;

namespace HotelListing.Api.Contracts;

public interface IUsersService
{
    Task<Result<string>> LoginUserAsync(LoginUserDto loginUserDto);
    Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto registerUserDto);
}