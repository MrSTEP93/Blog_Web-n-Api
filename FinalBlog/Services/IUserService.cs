using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using Microsoft.Identity.Client;

namespace FinalBlog.Services
{
    public interface IUserService
    {
        public Task<ResultModel> Register(RegistrationViewModel model);

        public Task<ResultModel> Login(LoginViewModel model);

        public Task<List<UserViewModel>> GetAllUsers();

        public Task<UserViewModel> GetUserById(string id);

        public Task<ResultModel> UpdateUserInfo(UserEditViewModel model);

        public Task<ResultModel> DeleteUser(string id);
    }
}
