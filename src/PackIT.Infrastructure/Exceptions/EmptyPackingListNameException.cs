using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Exceptions
{
    public class EmptyPackingListNameException : PackItException
    {
        public EmptyPackingListNameException() : base("packing list name cannot be empty.")
        {
        }
    }
}
