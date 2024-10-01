using AutoMapper;
using FinalBlog.Models;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class UserController(
        IMapper mapper,
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<BlogUser> roleManager) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly RoleManager<BlogUser> _roleManager = roleManager;

        [Route("UserList")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrationShortForm()
        {
            return View();
        }

        [Route("Registration")]
        [HttpPost]
        public IActionResult RegistrationFullForm(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // go to user service - registration
            }
            return View();
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // go to user service - login
            }
            return View();
        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
