using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.Services;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinalBlog.Controllers
{
    public class RoleController(
        //IMapper mapper,
        //UserManager<BlogUser> userManager,
        //SignInManager<BlogUser> signInManager,
        //RoleManager<Role> roleManager,
        IRoleService roleService) : Controller
    {
        //private readonly IMapper _mapper = mapper;
        //private readonly UserManager<BlogUser> _userManager = userManager;
        //private readonly SignInManager<BlogUser> _signInManager = signInManager;
        //private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly IRoleService _roleService = roleService;

        [HttpGet]
        public IActionResult Index()
        {
            //return View();
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> NewRole()
        {
            return BadRequest("Not supported");
            //return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.AddRole(model);
                if (result.IsSuccessed)
                {
                    return Ok("Роль успешно добавлена");
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }
            
            return BadRequest(ModelState);
            //return View();
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateRole(model);
                if (result.IsSuccessed)
                {
                    return Ok("Роль изменена");
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
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
