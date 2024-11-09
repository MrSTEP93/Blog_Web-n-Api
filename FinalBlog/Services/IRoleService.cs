using FinalBlog.ViewModels.Role;

namespace FinalBlog.Services
{
    public interface IRoleService
    {
        public Task AddRole(RoleViewModel model);
        public Task<RoleViewModel> UpdateRole(string roleId);
        public Task UpdateRole(RoleViewModel model);
        public Task DeleteRole(string roleId);
        public List<RoleViewModel> GetAllRoles();
    }
}
