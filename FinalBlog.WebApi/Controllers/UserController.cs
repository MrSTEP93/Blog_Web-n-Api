using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using FinalBlog.Data.Models;
using System.Security.Claims;

namespace FinalBlog.WebApi.Controllers
{
    /// <summary>
    /// Контроллер пользовательских данных
    /// </summary>
    /// <param name="userService">Подключаемый из DI сервис для работы с пользователями</param>
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Метод для получения всех пользователей
        /// </summary>
        [Authorize(Roles = "Администратор, Модератор")]
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var result = _userService.ConvertToViewModel(await _userService.GetAllUsers());
            return Ok(result);
        }

        /// <summary>
        /// Просмотр профиля пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        [HttpGet]
        public async Task<IActionResult> GetInfo(string? id)
        {
            if (!User.Identity!.IsAuthenticated)
                return BadRequest("Пользователь не авторизован для просмотра профиля");

            id ??= CurrentUserId;
            try
            {
                BlogUser userData = new();
                userData = await _userService.GetUserById(id!);
                return Ok(_userService.ConvertToViewModel(userData));
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновление данных пользователя
        /// </summary>
        /// <param name="model">Модель с данными пользователя</param>
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _userService.UpdateUserInfo(model);

                if (resultModel.IsSuccessed)
                    return Ok("Success");

                foreach (var message in resultModel.Messages)
                    ModelState.AddModelError("", message);
            }

            return BadRequest(model);
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (ModelState.IsValid)
            {
                var resultModel = await _userService.DeleteUser(id);
                if (resultModel.IsSuccessed)
                    return Ok(resultModel);
                else
                    return BadRequest(resultModel);
            }
            return BadRequest(ModelState);
        }
    }
}
