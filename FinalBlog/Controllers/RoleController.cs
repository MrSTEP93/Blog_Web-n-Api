using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.Services;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class RoleController(
        IRoleService roleService) : Controller
    {
        private readonly IRoleService _roleService = roleService;

        /// <summary>
        /// [GET] Вывод всех ролей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            var model = new RoleListViewModel()
            {
                Roles = _roleService.GetAllRoles()
            };

            return View("Roles", model);
        }

        /// <summary>
        /// [GET] Окно создания роли
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult NewRole() => View();

        /// <summary>
        /// [POST] Создания новой роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<IActionResult> Add(RoleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.AddRole(model);
                if (!result.IsSuccessed)
                {
                    foreach (var message in result.Messages)
                        ModelState.AddModelError("", message);
                }
                else
                {
                    return RedirectToAction("Index","Role");
                }
            }
            return View(model);
        }

        /// <summary>
        /// [GET] Окно редактирования роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Администратор")]
        [HttpGet]
        public IActionResult Edit(string id) => View(_roleService.GetRoleById(id));

        /// <summary>
        /// [POST] Редактирование роли
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Администратор")] 
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRole(model);
                if (!result.IsSuccessed)
                {
                    foreach (var message in result.Messages)
                        ModelState.AddModelError("", message);
                }
                else
                {
                    return RedirectToAction("Index", "Role");
                }
            }

            return View(model);
        }

        /// <summary>
        /// [GET] Подтверждение удаления роли
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id) => View(_roleService.GetRoleById(id));
        
        /// <summary>
        /// [POST] Удаление роли
        /// </summary>
        /// <param name="id"></param>
        /// <param name="confirmed"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id, string confirmed = "yes")
        {
            if (ModelState.IsValid && confirmed == "yes")
            {
                var result = await _roleService.DeleteRole(id);
                if (result.IsSuccessed)
                {
                    return Ok("Роль успешно удалена");
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
