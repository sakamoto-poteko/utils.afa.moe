namespace Moe.Afa.Utils.API.Services.Exceptions;

public class InvalidPhoneNumberException : Exception
{
    public InvalidPhoneNumberException(string message) : base(message)
    {
    }
}