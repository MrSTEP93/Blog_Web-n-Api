using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TEMP_Controller(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
