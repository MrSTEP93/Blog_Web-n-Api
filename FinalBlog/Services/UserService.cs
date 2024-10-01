using AutoMapper;
using FinalBlog.Models;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace FinalBlog.Services
{
    public class UserService(
        IMapper mapper,
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        RoleManager<BlogUser> roleManager)
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly RoleManager<BlogUser> _roleManager = roleManager;
        
        public async Task<ResultModel> Register(RegistrationViewModel model)
        {
            ResultModel resultModel = new(false);
            var newUser = _mapper.Map<BlogUser>(model);
            newUser.RegDate = DateTime.Now;

            var result = await _userManager.CreateAsync(newUser, model.RegPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                //await _roleManager.

                resultModel.IsSuccessed = true;
            } else
            {
                foreach (var error in result.Errors)
                {
                    resultModel.AddMessage(error.Description);
                }
            }

            return resultModel;
        }

        public async Task<ResultModel> Login(LoginViewModel model)
        {
            ResultModel resultModel = new(false);
            var user = _mapper.Map<BlogUser>(model);

            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                resultModel.IsSuccessed = true;
                resultModel.AddMessage("Authorization successed");
            }
            else
            {
                resultModel.AddMessage("Неправильный логин и (или) пароль");
            }

            return resultModel;
        }
    }

}
