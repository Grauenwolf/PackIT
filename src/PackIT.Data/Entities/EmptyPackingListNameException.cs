using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class EmptyPackingListNameException : PackItException
    {
        public EmptyPackingListNameException() : base("packing list name cannot be empty.")
        {
        }
    }
}
