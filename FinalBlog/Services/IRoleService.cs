using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Role;

namespace FinalBlog.Services
{
    public interface IRoleService
    {
        public Task<ResultModel> AddRole(RoleAddViewModel model);
        public RoleEditViewModel GetRoleById(string roleId);
        public RoleEditViewModel GetRoleByName(string roleName);
        public Task<ResultModel> UpdateRole(RoleEditViewModel model);
        public Task<ResultModel> DeleteRole(string roleId);
        public List<RoleEditViewModel> GetAllRoles();
    }
}
