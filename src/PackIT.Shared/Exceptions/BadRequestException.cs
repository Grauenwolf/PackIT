using Microsoft.AspNetCore.Http;

namespace PackIT.Shared.Exceptions
{
    public abstract class BadRequestException : HttpStatusException
    {
        protected BadRequestException(string message) : base(message, StatusCodes.Status400BadRequest)
        {
        }
    }
}
