using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Role;

namespace FinalBlog.Services
{
    public interface IRoleService
    {
        public Task<ResultModel> AddRole(RoleViewModel model);
        public Task<RoleViewModel> GetRoleById(string roleId);
        public Task<RoleViewModel> GetRoleByName(string roleName);
        public Task<ResultModel> UpdateRole(RoleViewModel model);
        public Task<ResultModel> DeleteRole(string roleId);
        public List<RoleViewModel> GetAllRoles();
    }
}
