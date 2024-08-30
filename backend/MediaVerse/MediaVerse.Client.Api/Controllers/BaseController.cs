using System.Diagnostics;
using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult ResolveException(Exception exception)
    {
        if (exception is NotFoundException)
        {
            return NotFound();
        }

        if (exception is ProblemException)
        {
            return Problem();
        }

        if (exception is ForbiddenException)
        {
            return Forbid();
        }

        if (exception is ConflictException)
        {
            return Conflict();
        }
        
        return Problem();
    }
    
}