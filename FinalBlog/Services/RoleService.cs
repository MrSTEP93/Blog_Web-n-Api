using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinalBlog.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(
            UserManager<BlogUser> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
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
            RoleAddViewModel userRoleModel = new()
            {
                Name = "Пользователь",
                Description = "Пользователь сайта"
            };
            AddRole(userRoleModel);

            RoleAddViewModel adminRoleModel = new()
            {
                Name = "Администратор",
                Description = "Администратор сайта"
            };
            AddRole(adminRoleModel);

            RoleAddViewModel moderatorRoleModel = new()
            {
                Name = "Модератор",
                Description = "Имеет право редактировать статьи"
            };
            AddRole(moderatorRoleModel);
        }

        public async Task<ResultModel> AddRole(RoleAddViewModel model)
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

        public RoleViewModel GetRoleById(string roleId)
        {
            var role = _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }
        
        public RoleViewModel GetRoleByName(string roleName)
        {
            var role = _roleManager.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }

        public async Task<ResultModel> UpdateRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return new ResultModel(false, "Роль с таким Id не обнаружена!");

            ResultModel resultModel = new(true, "Роль успешно обновлена");
            try
            {
                role.ConvertRole(model);
                var result = await _roleManager.UpdateAsync(role);
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return new ResultModel(false, "Роль с таким Id не обнаружена!");
            
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

        public async Task<List<Role>> GetRolesOfUser(BlogUser user)
        {
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var result = new List<Role>();
            foreach (var item in allRoles)
            {
                if (userRoleNames.Contains(item.Name!))
                    result.Add(item);
            }
            return result;
        }
    }
}
