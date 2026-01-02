namespace AuthService.Application.Common.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base() { }
        public ApplicationException(string message) : base(message) { }
        public ApplicationException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class ValidationException : ApplicationException
    {
        public ValidationException(string message) : base(message) { }

        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }

    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message) : base(message) { }
    }

    public class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message) : base(message) { }
    }
}
