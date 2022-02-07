using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class EmptyPackingListIdException : BadRequestException
    {
        public EmptyPackingListIdException() : base("Packing list ID cannot be empty.")
        {
        }
    }
}
