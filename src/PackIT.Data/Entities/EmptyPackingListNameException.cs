using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class EmptyPackingListNameException : BadRequestException
    {
        public EmptyPackingListNameException() : base("packing list name cannot be empty.")
        {
        }
    }
}
