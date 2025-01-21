using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Показ пользователю ошибки статус кода 404
        /// </summary>
        [HttpGet]
        [Route("NotFound")]
        public new IActionResult NotFound() => View();

        /// <summary>
        /// Показ пользователю ошибки статус кодов 400, 500
        /// </summary>
        [HttpGet]
        [Route("BadRequest")]
        public new IActionResult BadRequest() => View();

        /// <summary>
        /// Показ пользователю ошибки статус кода 403
        /// </summary>
        [HttpGet]
        [Route("AccessDenied")]
        public IActionResult AccessDenied() => View();
    }
}
