using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.DTOs.Auth;
using HotelListing.Api.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HotelListing.Api.Services;

public class UsersService(UserManager<ApplicationUser> userManager, IConfiguration configuration) : IUsersService
{
    public async Task<Result<RegisteredUserDto>> RegisterUserAsync(RegisterUserDto registerUserDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerUserDto.Email,
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName
        };

        var result = await userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            return Result<RegisteredUserDto>.BadRequest(errors);
        }

        await userManager.AddToRoleAsync(user, registerUserDto.Role);

        var registeredUser = new RegisteredUserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = registerUserDto.Role
        };

        return Result<RegisteredUserDto>.Success(registeredUser);
    }

    public async Task<Result<string>> LoginUserAsync(LoginUserDto loginUserDto)
    {
        var user = await userManager.FindByEmailAsync(loginUserDto.Email);

        if (user == null)
        {
            return Result<string>.Failure(new Error(ErrorCodes.BadRequest, "Invalid credentials."));
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!isPasswordValid)
        {
            return Result<string>.Failure(new Error(ErrorCodes.BadRequest, "Invalid credentials."));
        }

        var token = await GenerateToken(user);

        return Result<string>.Success(token);
    }


    private async Task<string> GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
    {
        new (JwtRegisteredClaimNames.Sub, user.Id),
        new (JwtRegisteredClaimNames.Email, user.Email),
        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new (JwtRegisteredClaimNames.Name, user.FullName)
    }; 

        // Add user roles as claims
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        claims = claims.Union(roleClaims).ToList();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(configuration["JwtSettings:DurationInMinutes"])),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
