
namespace TaskManagerAPI.DTOs{
    public class LoginHistoryDto
    {
        public int UserId { get; set; } // Foreign Key from Users
        public DateTime LoginDate { get; set; } = DateTime.Now; // Date and time of the login attempt
        public string? IPAddress { get; set; } // IP Address of the user
        public bool Success { get; set; } // Whether the login was successful (true/false)
    }
}