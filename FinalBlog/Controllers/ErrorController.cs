using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Показ пользователю ошибки статус кода 404
        /// [Route("NotFound")]
        /// </summary>
        [HttpGet]
        public new IActionResult NotFound() => View();

        /// <summary>
        /// Показ пользователю ошибки статус кодов 400, 500
        /// [Route("BadRequest")]
        /// </summary>
        [HttpGet]
        public new IActionResult BadRequest() => View();

        /// <summary>
        /// Показ пользователю ошибки статус кода 403
        /// [Route("AccessDenied")]
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
