using System;

namespace PackIT.Shared.Exceptions
{
    public abstract class HttpStatusException : Exception
    {
        protected HttpStatusException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
        public int StatusCode { get; set; }
    }
}
