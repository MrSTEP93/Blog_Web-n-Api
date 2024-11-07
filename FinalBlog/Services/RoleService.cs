using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.UoW;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FinalBlog.Services
{
    public class RoleService(
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<Role> roleManager,
        IMapper mapper,
        IUnitOfWork unitOfWork) : IRoleService
    {
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly RoleManager<Role> _roleManager = roleManager;
        private IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public Task AddRole(RoleViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoleViewModel>> GetAllRoles()
        {
            throw new NotImplementedException();
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
    }
}
