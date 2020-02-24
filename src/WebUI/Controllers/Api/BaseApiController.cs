using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
    }
}