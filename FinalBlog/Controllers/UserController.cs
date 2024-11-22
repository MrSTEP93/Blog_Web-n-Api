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
        IUserService userService) : Controller
    {
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
            if (ModelState.IsValid)
            {
                var resultModel = await _userService.Login(model);
                if (!resultModel.IsSuccessed)
                {
                    return BadRequest(resultModel);
                }
                return Ok("Login successfully");
            }
            return BadRequest(ModelState);
        }

        [Route("Logout")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Ok();
            //return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ShowUser(string id)
        {
            var model = await _userService.GetUserById(id);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShowUserEditForm()
        {
            var model = await _userService.GetCurrentUser(User);

            //return View();
            return Ok(model);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var resultModel = await _userService.DeleteUser(id);
            return Ok(resultModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            var model = _userService.CreateRandomUsers();

            return Ok(model);
        }
    }
}
