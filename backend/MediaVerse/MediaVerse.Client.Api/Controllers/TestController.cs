using MediatR;
using MediaVerse.Client.Application.Queries.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediaVerse.Client.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("test"), Authorize(Policy = "Admin")]
        public async Task<IActionResult> Test()
        {
            return Ok(_mediator.Send(new TestQuery()));
        }

    }
}
