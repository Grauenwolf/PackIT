using PackIT.Shared.Exceptions;

namespace PackIT.Data.Entities
{
    public class EmptyPackingListItemNameException : BadRequestException
    {
        public EmptyPackingListItemNameException() : base("Packing item name cannot be empty.")
        {
        }
    }
}
