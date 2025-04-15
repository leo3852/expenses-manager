using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerAPI.Models;
using TaskManagerAPI.DTOs;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoginHistoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/LoginHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginHistoryDto>>> GetLoginHistory()
        {
            var history = await _context.LoginHistory
                .Select(lh => new LoginHistoryDto
                {
                    UserId = lh.UserId,
                    LoginDate = lh.LoginDate,
                    IPAddress = lh.IPAddress,
                    Success = lh.Success
                }).ToListAsync();

            return Ok(history);
        }

        // POST: api/LoginHistory
        [HttpPost]
        public async Task<ActionResult<LoginHistoryDto>> PostLoginHistory(LoginHistoryDto loginHistoryDto)
        {
            var loginHistory = new LoginHistoryItem
            {
                UserId = loginHistoryDto.UserId,
                LoginDate = loginHistoryDto.LoginDate,
                IPAddress = loginHistoryDto.IPAddress,
                Success = loginHistoryDto.Success
            };

            _context.LoginHistory.Add(loginHistory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLoginHistory), new { id = loginHistory.Id }, loginHistoryDto);
        }
    }
}
