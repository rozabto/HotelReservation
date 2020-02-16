using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class BaseController : Controller
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= (IMediator)HttpContext.RequestServices.GetService(typeof(IMediator));
    }
}
