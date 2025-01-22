using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TEMP_Controller(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
