using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using CrimeSyndicate.DbContexts;
using CrimeSyndicate.Models;
using CrimeSyndicate.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace CrimeSyndicate.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly CrimeContext _db;
    private readonly UserRepo _userRepo;

    public UserController(CrimeContext db)
    {
        _db = db;
        _userRepo = new UserRepo(db.Users);
    }

    [HttpPost]
    [Route("new")]
    public async Task<IActionResult> CreateUser(User user)
    {
        await user.GeneratePassword();
        
        _userRepo.Add(user);

        int numChanges;
        try {
            numChanges = await _db.SaveChangesAsync();
        } catch (DbUpdateException e) {
            var inner = e.InnerException as PostgresException;
            return BadRequest(inner?.SqlState == PostgresErrorCodes.UniqueViolation ? "Username or email is already taken" : inner?.ToString());
        }
        
        if (numChanges < 1)
        {
            return BadRequest("User could not be created");
        }

        return Ok($"User {user.Name} successfully created");
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        HttpContext.Session.Set("HasAttemptedLogin", new []{ (byte)1 });
        var user = await _userRepo.FindUserByName(req.Name);
        if (user == null)
        {
            return NotFound("User does not exist");
        }
        
        var loginSuccess = await user.VerifyPassword(req.Password);
        if (!loginSuccess)
        {
            return Forbid("Incorrect password");
        }
        
        var claims = new Claim[]
        {
            new(ClaimTypes.Hash, user.Id.ToString()),
            new(ClaimTypes.Name, user.Name)
        };
        var claimsIdentity = new ClaimsIdentity(claims);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity));

        return Ok("Login successful");
    }
}

public class LoginRequest
{
    [Required] public string Name { get; set; }
    [Required] public string Password { get; set; }
}