namespace Application.Exceptions;

using System.Net;

public class ForbiddenException(
    string message,
    List<string> errorMessages = default,
    HttpStatusCode statusCode = HttpStatusCode.Conflict)
    : Exception(message)
{
    public List<string> ErrorMessages { get; set; } = errorMessages;
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}
