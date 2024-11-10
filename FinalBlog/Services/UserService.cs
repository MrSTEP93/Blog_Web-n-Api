using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace FinalBlog.Services
{
    public class UserService(
        IMapper mapper,
        UserManager<BlogUser> userManager,
        SignInManager<BlogUser> signInManager,
        IRoleService roleService,
        IUnitOfWork unitOfWork
        ) : IUserService
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> Register(RegistrationViewModel model)
        {
            ResultModel resultModel = new(false);
            var newUser = _mapper.Map<BlogUser>(model);

            var result = await _userManager.CreateAsync(newUser, model.RegPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                result = await _userManager.AddToRoleAsync(newUser, "Пользователь");
                if (result.Succeeded)
                {
                    resultModel.MarkAsSuccess();
                    return resultModel;
                }
            }
            resultModel.FillMessagesFromResult(result);
            
            return resultModel;
        }

        public async Task<ResultModel> Login(LoginViewModel model)
        {
            ResultModel resultModel = new(false);
            var user = _mapper.Map<BlogUser>(model);

            var result = await _signInManager.PasswordSignInAsync(user.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                resultModel.MarkAsSuccess("Успешная авторизация");
            else
            {
                resultModel.AddMessage("Неправильный логин и (или) пароль");
            }

            return resultModel;
        }

        public List<BlogUser> GetUserList()
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            return repository.GetAll().ToList();
        }

        public async Task<BlogUser> GetUser(string id)
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            return await repository.Get(id);
        }

        public async Task<ResultModel> UpdateUserInfo(UserEditViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            user.ConvertUser(model);
            var result = await _userManager.UpdateAsync(user);

            ResultModel resultModel = new(in result, "Данные успешно обновлены");
            //resultModel.ProcessResult();
            return resultModel;
        }

        public async Task<ResultModel> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new ResultModel(false, "User not found");
            }
            var result = await _userManager.DeleteAsync(user);

            ResultModel resultModel = new(in result, "Пользователь удален");
            //resultModel.ProcessResult();
            return resultModel;
        }
    }

}
