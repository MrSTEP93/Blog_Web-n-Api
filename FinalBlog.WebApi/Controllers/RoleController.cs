using FinalBlog.Services.Interfaces;
using FinalBlog.Services;
using Microsoft.AspNetCore.Mvc;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Authorization;

namespace FinalBlog.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RoleController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        /// <summary>
        /// [GET] Вывод всех ролей
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }

        /// <summary>
        /// [POST] Создания новой роли
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpPost]
        public async Task<IActionResult> Add(RoleAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.AddRole(model);
                if (result.IsSuccessed)
                    return Ok("Роль успешно добавлена");
                else
                    return BadRequest(result.Messages);
            }

            return BadRequest(model);
        }

        /// <summary>
        /// [PUT] Редактирование роли
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpPut]
        public async Task<IActionResult> Edit(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRole(model);
                if (result.IsSuccessed)
                    return Ok("Роль изменена");
                else
                    return BadRequest(result.Messages);
            }

            return BadRequest(model);
        }

        /// <summary>
        /// [DELETE] Удаление роли
        /// </summary>
        [Authorize(Roles = "Администратор")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.DeleteRole(id);
                if (result.IsSuccessed)
                    return Ok("Роль успешно удалена");
                else
                    return BadRequest(result);
            }

            return BadRequest(ModelState);
        }
    }
}
