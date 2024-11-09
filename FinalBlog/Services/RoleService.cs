using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
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
        private IMapper _mapper;
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

        public async Task AddRole(RoleViewModel model)
        {
            Role newRole = new()
            {
                Name = model.Name,
                Description = model.Description
            };
            await _roleManager.CreateAsync(newRole);
        }

        public Task<RoleViewModel> UpdateRole(string roleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRole(RoleViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRole(string roleId)
        {
            throw new NotImplementedException();
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
