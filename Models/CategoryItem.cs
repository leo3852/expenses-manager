using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public class CategoryItem
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Icon { get; set; }
    }
}
