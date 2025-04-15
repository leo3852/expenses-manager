using TaskManagerAPI.Models;

public class LoginHistoryItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime LoginDate { get; set; }
    public string? IPAddress { get; set; }
    public bool Success { get; set; }

    public UserItem? User { get; set; } // Navigation property to the User entity
}
