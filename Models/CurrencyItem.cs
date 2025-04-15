namespace TaskManagerAPI.Models
{
    public class CurrencyItem
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; 
        public string Symbol { get; set; } = string.Empty; 
        public string Name { get; set; } = string.Empty; 
    }
}