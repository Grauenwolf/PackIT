using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Exceptions
{
    public class PackingItemNotFoundException : PackItException
    {
        public string ItemName { get; }

        public PackingItemNotFoundException(string itemName) : base($"Packing item '{itemName}' was not found.")
            => ItemName = itemName;
    }
}
