using AutoMapper;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.Extensions;
using FinalBlog.Services;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class UserController(
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<Role> roleManager,
        IUserService userService) : Controller
    {
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly IUserService _userService = userService;

        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //return View();
            var result = await _userService.GetAllUsers();
            return Ok(result);
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
            return BadRequest("not ready");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.IsSuccessed)
                {
                    return Ok("Registration successfull");
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }
            return BadRequest(ModelState);
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
                        ModelState.AddModelError("", message);
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

        [HttpGet]
        public async Task<IActionResult> ShowUser(string id)
        {
            //var result = await _userManager.FindByIdAsync(id);
            var model = await _userService.GetUserById(id);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowUserEditForm()
        {
            var result = await _userManager.GetUserAsync(User);

            //var model = new UserViewModel(result);
            //return View();
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserEditViewModel model) 
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _userService.UpdateUserInfo(model);
                if (!resultModel.IsSuccessed)
                {
                    foreach (var message in resultModel.Messages)
                    {
                        ModelState.AddModelError("", message);
                    }
                } 
                else
                    return Ok("Updated");
            }
            
            return BadRequest(ModelState);
            //return View("UserEdit", model);
        }

        [Route("DeleteUser")]
        [HttpPost]
        public IActionResult AskDeleteConfirm()
        {
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var resultModel = await _userService.DeleteUser(id);
            return Ok(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            byte userCount = 5;
            var usergen = new UserGenerator();
            var userlist = usergen.Populate(userCount);

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user, "123456");

                if (!result.Succeeded)
                {
                    return StatusCode(500, result);
                }
            }

            return Ok($"The method has created {userCount} users (hopefully)");
        }
    }
}
