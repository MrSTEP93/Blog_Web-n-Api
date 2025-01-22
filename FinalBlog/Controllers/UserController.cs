using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Azure.Core;
using FinalBlog.Data.Models;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace FinalBlog.Controllers
{
    public class UserController(
        IUserService userService,
        IMapper mapper,
        ILogger<UserController> logger
        ) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;
        string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// [GET] Метод для получения всех пользователей
        /// </summary>
        /// <param name="viewMode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// [GET] Отображение формы регистрации
        /// </summary>
        /// <returns></returns>
        [Route("Register")]
        [HttpGet]
        public IActionResult Register() => View();

        /// <summary>
        /// [POST] Обработка данных с формы регистрации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Register(model);
                if (result.IsSuccessed)
                {
                    _logger.LogInformation($"Создан аккаунт: {model.Email}");
                    return RedirectToAction("Index", "Home");
                }
                
                foreach (var message in result.Messages)
                {
                    _logger.LogError($"Ошибка при регистрации {{ {model.Email} }}: {message}");
                    ModelState.AddModelError("", message);
                }
            }
            return View("Register", model);
        }

        /// <summary>
        /// [GET] Отображение формы входа (авторизации)
        /// </summary>
        /// <returns></returns>
        [Route("Login")]
        [HttpGet]
        public IActionResult Login() => View();

        /// <summary>
        /// Метод авторизации
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                    _logger.LogError($"Ошибка авторизации в UserService: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        _logger.LogError($"Ошибка авторизации в UserService (внутреннее исключение): {ex.Message}");
                        ModelState.AddModelError("", ex.Message);
                    }
                }
                if (!resultModel.IsSuccessed)
                {
                    foreach (var message in resultModel.Messages)
                    {
                        _logger.LogError($"Ошибка авторизации: {message}");
                        ModelState.AddModelError("", message);
                    }
                } else
                {
                    _logger.LogInformation($"Пользователь авторизован: {model.Email}");
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        /// <summary>
        /// [POST] Выход из системы
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// [GET] Просмотр страницы пользователя
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Info(string id)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                _logger.LogInformation($"Пользователь не авторизован для просмотра профиля {{ {id} }}");
                return View("AccessDenied");
                //return RedirectToAction("Index", "Home");
            }

            BlogUser userData;
            if (id == null)
                userData = await _userService.GetCurrentUser(User);
            else
                userData = await _userService.GetUserById(id);
            
            return View(userData.ToViewModel(_mapper));
        }

        /// <summary>
        /// [GET] Страница редактирования информации пользователя
        /// </summary>
        /// <param name="id">ID пользователя (если NULL - пользователь редактирует свой профиль)</param>
        /// <returns></returns>
        [Route("Profile")]
        [HttpGet]
        public async Task<IActionResult> ShowUserEditForm(string? id = null)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                _logger.LogInformation($"Пользователь не авторизован для редактирования профиля {{ {id} }}");
                return View("AccessDenied");
                //return RedirectToAction("Index", "Home");
            }

            BlogUser userData;
            if (id == null)
                userData = await _userService.GetCurrentUser(User);
            else
                userData = await _userService.GetUserById(id);

            return View("Edit", userData.ToViewModel(_mapper));
        }

        /// <summary>
        /// [POST] Сохранение изменений пользователя
        /// </summary>
        /// <param name="model">Модель с данными пользователя</param>
        /// <param name="newRoles">Список выбранных ролей</param>
        /// <returns></returns>
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
                {
                    _logger.LogInformation($"Ошибка при изменении данных пользователя: {message}");
                    ModelState.AddModelError("", message);
                }
            }
            
            return View("Edit", model);
        }

        /// <summary>
        /// Удаление
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var resultModel = await _userService.DeleteUser(id);
            _logger.LogInformation($"Администратор {{ {CurrentUserId} }} удалил пользователя ID={id}");
            return Ok(resultModel);
        }
        
        /// <summary>
        /// Служебный метод для создания случайных пользователей
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public async Task<IActionResult> CreateUsers()
        {
            var model = await _userService.CreateRandomUsers();

            return Ok(model);
        }
    }
}
