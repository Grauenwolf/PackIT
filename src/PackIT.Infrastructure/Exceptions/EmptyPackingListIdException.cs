using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Exceptions
{
    public class EmptyPackingListIdException : PackItException
    {
        public EmptyPackingListIdException() : base("Packing list ID cannot be empty.")
        {
        }
    }
}