namespace PackIT.Infrastructure.Queries.Models
{
    public class PackingItemDto
    {
        public string Name { get; set; }
        public uint Quantity { get; set; }
        public bool IsPacked { get; set; }
    }
}