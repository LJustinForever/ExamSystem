using ExamSystem.Model;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExamSystem.Services;

public static class AuthEndpoints
{
    public static void AddAuthApi(this WebApplication app)
    {
        app.MapPost("api/Register", async (UserManager<ApplicationUser> userManager, RegisterUserDto registerUserDto) =>
        {
            var user = await userManager.FindByNameAsync(registerUserDto.Username);
            if (user != null)
            {
                return Results.UnprocessableEntity("Username already taken.");
            }
            var newUser = new ApplicationUser
            {
                Email = registerUserDto.Email,
                UserName = registerUserDto.Username,
                Name = registerUserDto.Name,
                LastName = registerUserDto.LastName,

            };
            var createUser = await userManager.CreateAsync(newUser, registerUserDto.Password);

            if (!createUser.Succeeded)
            {
                return Results.UnprocessableEntity();
            }

            await userManager.AddToRoleAsync(newUser, UserRoles.USER);

            return Results.Created("api/Login", new UserDto(newUser.Id, newUser.UserName, newUser.Email));
        });

        app.MapPost("api/Login", async (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, LoginUserDto loginUserDto) =>
        {
            var user = await userManager.FindByNameAsync(loginUserDto.Username);
            if (user == null)
            {
                return Results.UnprocessableEntity("Username or password was incorrect.");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);

            if (!isPasswordValid)
            {
                return Results.UnprocessableEntity("Username or password was incorrect.");
            }
            user.ForceRelogin = false;
            await userManager.UpdateAsync(user);
            await signInManager.SignInAsync(user, isPersistent: true);
            var roles = await userManager.GetRolesAsync(user);
            var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var refreshToken = jwtTokenService.CreateRefreshToken(user.Id);


            return Results.Ok(new SuccesfulLoginDto(accessToken, refreshToken));
        });


        app.MapPost("api/AccessToken", async (UserManager<ApplicationUser> userManager, JwtTokenService jwtTokenService, RefreshAccessTokenDto refreshAccessTokenDto) =>
        {
            if (!jwtTokenService.TryParseRefreshToken(refreshAccessTokenDto.RefreshToken, out var claims))
            {
                return Results.UnprocessableEntity();
            }

            var userID = claims.FindFirstValue(JwtRegisteredClaimNames.Sub);

            var user = await userManager.FindByIdAsync(userID);

            if (user == null)
            {
                return Results.UnprocessableEntity("Invalid token.");
            }

            if (user.ForceRelogin)
            {
                return Results.UnprocessableEntity();
            }

            var roles = await userManager.GetRolesAsync(user);
            var accessToken = jwtTokenService.CreateAccessToken(user.UserName, user.Id, roles);
            var refreshToken = jwtTokenService.CreateRefreshToken(user.Id);

            return Results.Ok(new SuccesfulLoginDto(accessToken, refreshToken));



        });

    }
}

public record RegisterUserDto(string Username, string Name, string LastName, string Email, string Password);
public record UserDto(Guid UserID, string Username, string Email);
public record LoginUserDto(string Username, string Password);
public record SuccesfulLoginDto(string AccessToken, string RefreshToken);
public record RefreshAccessTokenDto(string RefreshToken);
