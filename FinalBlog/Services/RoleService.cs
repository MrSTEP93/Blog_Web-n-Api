using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinalBlog.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly SignInManager<BlogUser> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(
            UserManager<BlogUser> userManager,
            SignInManager<BlogUser> signInManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            var repository = _unitOfWork.GetRepository<Role>() as RoleRepository;
            if (!repository.GetAll().Any())
            {
                CreateStartRoles();
            }
        }

        private void CreateStartRoles()
        {
            RoleViewModel userRoleModel = new()
            {
                Name = "Пользователь",
                Description = "Пользователь сайта"
            };
            AddRole(userRoleModel);

            RoleViewModel adminRoleModel = new()
            {
                Name = "Администратор",
                Description = "Администратор сайта"
            };
            AddRole(adminRoleModel);
        }

        public async Task<ResultModel> AddRole(RoleViewModel model)
        {
            Role newRole = new()
            {
                Name = model.Name,
                Description = model.Description
            };
            var result = await _roleManager.CreateAsync(newRole);
            ResultModel resultModel = new(in result);
            //resultModel.ProcessResult(in result);
            return resultModel;
        }

        public async Task<RoleViewModel> GetRoleById(string roleId)
        {
            var role = _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }
        
        public async Task<RoleViewModel> GetRoleByName(string roleName)
        {
            var role = _roleManager.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }

        public async Task<ResultModel> UpdateRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.ID);
            role.ConvertRole(model);
            var result = await _roleManager.UpdateAsync(role);

            ResultModel resultModel = new(in result, "Данные успешно обновлены");
            //resultModel.ProcessResult();
            return resultModel;
        }

        public async Task<ResultModel> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var result = await _roleManager.DeleteAsync(role);
            ResultModel resultModel = new(in result, "Роль удалена");
            return resultModel;
        }

        public List<RoleViewModel> GetAllRoles()
        {
            var repo = _unitOfWork.GetRepository<Role>() as RoleRepository;
            var roles = repo.GetAll();
            var rolesView = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleViewModel>(role));
            }
            return rolesView;
        }
    }
}
