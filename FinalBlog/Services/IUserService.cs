using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace FinalBlog.Services
{
    public interface IUserService
    {
        public Task<ResultModel> Register(RegistrationViewModel model);

        public Task<ResultModel> Login(LoginViewModel model);

        public Task Logout();

        public Task<List<UserViewModel>> GetAllUsers();

        public Task<UserViewModel> GetUserById(string id);

        public Task<UserViewModel> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        public Task<ResultModel> UpdateUserInfo(UserEditViewModel model);

        public Task<ResultModel> DeleteUser(string id);

        public Task<ResultModel> CreateRandomUsers();
    }
}
