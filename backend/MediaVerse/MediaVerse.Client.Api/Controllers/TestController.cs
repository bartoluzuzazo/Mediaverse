using MediatR;
using MediaVerse.Client.Application.Queries.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController(IMediator mediator) : ControllerBase
    {
        [HttpGet("test"), Authorize(Policy = "Admin")]
        public async Task<IActionResult> Test()
        {
            return Ok(mediator.Send(new TestQuery()));
        }

    }
}
