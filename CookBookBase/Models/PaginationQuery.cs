namespace CookBookBase.Models
{
    public class PaginationQuery
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public string? RecipeName { get; set; }
        public List<string>? Tags { get; set; }
    }
}
