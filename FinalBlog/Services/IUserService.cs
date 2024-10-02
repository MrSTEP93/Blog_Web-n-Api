using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using Microsoft.Identity.Client;

namespace FinalBlog.Services
{
    public interface IUserService
    {
        public Task<ResultModel> Register(RegistrationViewModel model);

        public Task<ResultModel> Login(LoginViewModel model);

        public Task<List<BlogUser>> GetUserList();

        public Task<BlogUser> GetUser(string id);

        public Task<ResultModel> UpdateUserInfo(UserEditViewModel model);
    }
}
