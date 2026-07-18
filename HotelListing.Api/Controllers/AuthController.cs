using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Controllers;
using HotelListing.Api.Domain;
using HotelListing.Api.Common.Results;
using HotelListing.Api.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.Api.Controller;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController(IUsersService userService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisteredUserDto>> Register(RegisterUserDto registerUserDto)
    {
        var result = await userService.RegisterUserAsync(registerUserDto);

        return ToActionResult(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
    {
        var result = await userService.LoginUserAsync(loginUserDto);

        return ToActionResult(result);
    }
}

