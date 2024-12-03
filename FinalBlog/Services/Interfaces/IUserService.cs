using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace FinalBlog.Services.Interfaces
{
    public interface IUserService
    {
        public Task<ResultModel> Register(RegistrationViewModel model);

        public Task<ResultModel> Login(LoginViewModel model);

        public Task<ResultModel> LoginWithClaims(LoginViewModel model);

        public Task Logout();

        public List<Claim> GetUserClaims(BlogUser user);

        public Task<List<BlogUser>> GetAllUsers();

        public Task<BlogUser> GetUserById(string id);

        public Task<BlogUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal);

        public Task<ResultModel> UpdateUserInfo(UserViewModel model);

        public Task<ResultModel> DeleteUser(string id);

        public Task<ResultModel> CreateRandomUsers();
    }
}
