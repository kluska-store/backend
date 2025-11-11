using System.Net;

namespace KluskaStore.Application.Exceptions;

public class ApplicationException(string message) : Exception(message) { }