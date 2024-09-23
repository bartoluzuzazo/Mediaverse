using MediaVerse.Domain.AggregatesModel;
using MediaVerse.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult ResolveCode(Exception? exception, IActionResult result)
    {
        return exception is not null ? ResolveException(exception) : result;
    }

    protected IActionResult OkOrError<T>(BaseResponse<T> response)
    {
        return ResolveCode(response.Exception, Ok(response.Data));
    }

    protected IActionResult OkOrError(Exception? exception)
    {
        return ResolveCode(exception, Ok());
    }

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