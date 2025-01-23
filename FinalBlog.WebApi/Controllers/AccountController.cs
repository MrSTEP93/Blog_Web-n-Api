using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using FinalBlog.ViewModels.User;
using FinalBlog.Data.Models;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        /// <summary>
        /// [POST] Обработка данных с формы регистрации
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _userService.Register(model);
                if (resultModel.IsSuccessed)
                {
                    return Ok(resultModel);
                }

                foreach (var message in resultModel.Messages)
                {
                    ModelState.AddModelError("", message);
                }
            }
            return BadRequest(model);
        }

        /// <summary>
        /// [POST] Метод авторизации
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var resultModel = await _userService.LoginWithClaims(model);
                    if (!resultModel.IsSuccessed)
                        return BadRequest(resultModel);
                }
                catch (Exception ex)
                {
                    var resultModel = new ResultModel(false);
                    resultModel.Messages.Add(ex.Message);
                    if (ex.InnerException != null)
                        resultModel.Messages.Add(ex.InnerException.Message);
                    return BadRequest(resultModel);
                }
                return Ok("Login successfully");
            }
                
            return BadRequest(model);
        }

        /// <summary>
        /// [POST] Выход из системы
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Ok("Logged out");
        }
    }
}
