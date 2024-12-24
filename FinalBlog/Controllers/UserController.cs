using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Azure.Core;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FinalBlog.Controllers
{
    public class UserController(
        IUserService userService,
        IMapper mapper) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        [Authorize(Roles = "Администратор, Модератор")]
        [Route("UserList")]
        [HttpGet]
        public async Task<IActionResult> UserList(string viewMode = "block")
        {
            var allUsers = await _userService.GetAllUsers();
            var result = new UserListViewModel()
            {
                Users = allUsers.ToViewModel(_mapper),
                ViewMode = viewMode
            };
            return View(result);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Info(string id)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            BlogUser userData;
            if (id == null)
                userData = await _userService.GetCurrentUser(User);
            else
                userData = await _userService.GetUserById(id);
            
            return View(userData.ToViewModel(_mapper));
        }

        [Route("Profile")]
        [HttpGet]
        public async Task<IActionResult> ShowUserEditForm(string? id = null)
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            BlogUser userData;
            if (id == null)
                userData = await _userService.GetCurrentUser(User);
            else
                userData = await _userService.GetUserById(id);

            return View("Edit", userData.ToViewModel(_mapper));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserViewModel model, List<string> newRoles) 
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel;
                if (newRoles.Count != 0)
                    resultModel = await _userService.UpdateUserInfo(model, newRoles);
                else 
                    resultModel = await _userService.UpdateUserInfo(model);

                if (resultModel.IsSuccessed)
                    return RedirectToAction("ShowUserEditForm", "User", model);

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }
            
            return View("Edit", model);
        }

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
