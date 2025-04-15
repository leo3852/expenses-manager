using System.ComponentModel.DataAnnotations;
using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Models
{
    public class UserItem
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public CurrencyDto? Currency { get; set; }

    }
}
