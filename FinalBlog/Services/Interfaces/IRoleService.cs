using FinalBlog.Data.Models;
using FinalBlog.ViewModels.Role;

namespace FinalBlog.Services.Interfaces
{
    public interface IRoleService
    {
        public Task<ResultModel> AddRole(RoleAddViewModel model);
        public RoleViewModel GetRoleById(string roleId);
        public RoleViewModel GetRoleByName(string roleName);
        public Task<ResultModel> UpdateRole(RoleViewModel model);
        public Task<ResultModel> DeleteRole(string roleId);
        public List<RoleViewModel> GetAllRoles();
        public Task<List<Role>> GetRolesOfUser(BlogUser user);
    }
}
