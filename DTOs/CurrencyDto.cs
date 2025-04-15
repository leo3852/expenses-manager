using TaskManagerAPI.Models;

namespace TaskManagerAPI.DTOs
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; 
        public string Symbol { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty;

        public static implicit operator CurrencyDto?(CurrencyItem? v)
        {
            throw new NotImplementedException();
        }
    }
}