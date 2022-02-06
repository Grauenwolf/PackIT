using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class EmptyPackingListIdException : PackItException
    {
        public EmptyPackingListIdException() : base("Packing list ID cannot be empty.")
        {
        }
    }
}
