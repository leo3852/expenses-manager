using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class TransactionItem
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string? Type { get; set; } // "Income" or "Expense"
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
