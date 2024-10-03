namespace BankingSystem.Domain.Exceptions
{
    [Serializable]
    public class BankingException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public BankingException(string message, ErrorCode errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public enum ErrorCode
    {
        Unknown = 0,

        Validation = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        Conflict = 409,
        LargeRequest = 413,

        Internal = 500
    }
}
