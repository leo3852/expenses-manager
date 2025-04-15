using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerAPI.DTOs;
using TaskManagerAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already in use");

        var currencyExists = await _context.Currencies.AnyAsync(c => c.Id == dto.CurrencyId);
        if (!currencyExists)
            return BadRequest("Invalid currency selection");

        var user = new UserItem
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow,
            CurrencyId = dto.CurrencyId
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(user);
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        if (dto == null)
            return BadRequest("Login data is required.");

        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest("Email and password are required.");

        if (!IsValidEmail(dto.Email))
            return BadRequest("Invalid email format.");

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
        bool loginSuccess = false;

        try
        {
            if (user != null && BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                var currencyEntity = await _context.Currencies.SingleOrDefaultAsync(c => c.Id == user.CurrencyId);

                CurrencyDto? currency = null;
                if (currencyEntity != null)
                {
                    currency = new CurrencyDto
                    {
                        Id = currencyEntity.Id,
                        Code = currencyEntity.Code,
                        Name = currencyEntity.Name,
                        Symbol = currencyEntity.Symbol
                    };
                }

                loginSuccess = true;
                var token = GenerateJwtToken(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    Currency = currency
                };

                await AddLoginHistoryAsync(user, loginSuccess);
                return Ok(new { token, userDto });
            }

            await AddLoginHistoryAsync(user, loginSuccess);
            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private async Task AddLoginHistoryAsync(UserItem? user, bool loginSuccess)
    {
        var loginHistoryItem = new LoginHistoryItem
        {
            UserId = user?.Id ?? 0, // if user is null (wrong email), save 0 or handle as anonymous
            LoginDate = DateTime.UtcNow,
            IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
            Success = loginSuccess
        };
        
        _context.LoginHistory.Add(loginHistoryItem);
        await _context.SaveChangesAsync();  // Save changes to the database
    }

    private string GenerateJwtToken(UserItem user)
    {
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
