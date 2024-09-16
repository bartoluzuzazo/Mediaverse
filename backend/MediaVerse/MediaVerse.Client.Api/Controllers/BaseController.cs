using System.Diagnostics;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult ResolveException(Exception exception)
    {
        return exception switch
        {
            NotFoundException => NotFound(),
            ProblemException => Problem(),
            ForbiddenException => Forbid(),
            ConflictException => Conflict(),
            _ => Problem()
        };
    }
}