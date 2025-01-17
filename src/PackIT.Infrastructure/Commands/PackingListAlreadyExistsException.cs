using PackIT.Shared.Exceptions;

namespace PackIT.Infrastructure.Commands
{
    public class PackingListAlreadyExistsException : BadRequestException
    {
        public string Name { get; }

        public PackingListAlreadyExistsException(string name) 
            : base($"Packing list with name '{name}' already exists.")
        {
            Name = name;
        }
    }
}