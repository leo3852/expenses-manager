

using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs{
    public class LoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

}
