using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.Extensions;
using FinalBlog.Services;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class UserController(
        IMapper mapper,
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<BlogUser> roleManager,
        IUserService userService) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly RoleManager<BlogUser> _roleManager = roleManager;
        private readonly IUserService _userService = userService;

        [Route("UserList")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegistrationShortForm()
        {
            return View();
        }

        [Route("Registration")]
        [HttpGet]
        public IActionResult RegistrationFullForm(RegistrationViewModel model)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegistrationViewModel model)
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //ResultModel resultModel = new(false);
            if (ModelState.IsValid)
            {
                // go to user service
                var resultModel = await _userService.Login(model);
                if (!resultModel.IsSuccessed)
                {
                    foreach (var message in resultModel.Messages)
                    {
                        ModelState.AddModelError("", message);
                    }
                }
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

        [Route("EditUser")]
        [HttpPost]
        public async Task<IActionResult> ShowUserEditForm()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserEditViewModel model) 
        {
            //ResultModel resultModel = new(false);
            if (ModelState.IsValid)
            {
                // go to user service
                var resultModel = await _userService.UpdateUserInfo(model);
                if (!resultModel.IsSuccessed)
                {
                    foreach (var message in resultModel.Messages)
                    {
                        ModelState.AddModelError("", message);
                    }
                }
            }
            
            return View("UserEdit", model);
        }

        [Route("DeleteUser")]
        [HttpPost]
        public IActionResult AskDeleteConfirm()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteUser(string id)
        {
            // go to user service
            _userService.DeleteUser(id);

            return View();
        }
    }
}
