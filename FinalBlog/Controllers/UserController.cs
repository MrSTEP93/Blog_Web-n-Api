using AutoMapper;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.Extensions;
using FinalBlog.Services;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class UserController(
        IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;


        [Authorize(Roles = "Администратор, Модератор")]
        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //return View();
            var result = await _userService.GetAllUsers();
            return Ok(result);
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.IsSuccessed)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(result);
                }
            }
            return View(ModelState);
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login() => View();

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                try
                {
                    resultModel = await _userService.LoginWithClaims(model);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    if (ex.InnerException != null)
                        ModelState.AddModelError("", ex.Message);
                }
                if (!resultModel.IsSuccessed)
                {
                    foreach (var message in resultModel.Messages)
                        ModelState.AddModelError("", message);
                } else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [Route("Logout")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            //return Ok();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Info(string id)
        {
            var model = await _userService.GetUserById(id);

            return View(model);
        }

        [Route("Profile")]
        [HttpGet]
        public async Task<IActionResult> ShowUserEditForm(string? id = null)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            UserViewModel model;

            if (id == null)
                model = await _userService.GetCurrentUser(User);
            else
                model = await _userService.GetUserById(id);

            return View("Edit", model);
            //return View();
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
                        ModelState.AddModelError("", message);
                } 
                else
                    return Ok("Updated");
            }
            
            //return View("UserEdit", model);
            return BadRequest(ModelState);
        }


        [Route("DeleteUser")]
        [HttpPost]
        public IActionResult AskDeleteConfirm()
        {
            return View();
        }

        [Authorize(Roles = "Администратор")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var resultModel = await _userService.DeleteUser(id);
            return Ok(resultModel);
        }
        
        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            var model = await _userService.CreateRandomUsers();

            return Ok(model);
        }
    }
}
