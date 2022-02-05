using System;

namespace PackIT.Shared.Exceptions
{
    public abstract class PackItException : Exception
    {
        protected PackItException(string message) : base(message)
        {
        }
    }
}