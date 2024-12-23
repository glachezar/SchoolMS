﻿namespace Application.Exceptions;

using System.Net;

public class IdentityException(
    string message, 
    List<string> errorMessages = default, 
    HttpStatusCode statusCode = HttpStatusCode.InternalServerError) 
    : Exception(message)
{
    public List<string> ErrorMessages { get; set; } = errorMessages;
    public HttpStatusCode StatusCode { get; set; } = statusCode;
}
