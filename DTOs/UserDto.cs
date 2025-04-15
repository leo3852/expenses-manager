using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }

        [Required]
        public int CurrencyId { get; set; } // Foreign Key
        public CurrencyDto? Currency { get; set; }
    }
}
