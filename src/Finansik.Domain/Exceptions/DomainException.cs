namespace Finansik.Domain.Exceptions;

public class DomainException : Exception
{
    public int ErrorCode { get; }
    
    public override string Message { get; }

    protected DomainException(int errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }
}