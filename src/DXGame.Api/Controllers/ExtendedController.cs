using Microsoft.AspNetCore.Mvc;

namespace DXGame.Api.Controllers
{
    public abstract class ExtendedController : Controller
    {
        public IActionResult ServiceUnavailable() 
        {
            return new StatusCodeResult(503);
        }

        public IActionResult InternalServerError()
        {
            return new StatusCodeResult(500);
        }
    }
}