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
        //RoleManager<Role> roleManager,
        IRoleService roleService,
        IUnitOfWork unitOfWork
        ) : IUserService
    {
        private readonly IMapper _mapper = mapper;
        private readonly UserManager<BlogUser> _userManager = userManager;
        private readonly SignInManager<BlogUser> _signInManager = signInManager;
        //private readonly RoleManager<Role> _roleManager = roleManager;
        private readonly IRoleService _roleService = roleService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<ResultModel> Register(RegistrationViewModel model)
        {
            ResultModel resultModel = new(false);
            var newUser = _mapper.Map<BlogUser>(model);
            newUser.RegDate = DateTime.Now;

            var result = await _userManager.CreateAsync(newUser, model.RegPassword);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, false);
                var userRole = _roleService.GetAllRoles().Where(r => r.Name == "Пользователь").FirstOrDefault();
                if (userRole == null)
                {
                    //throw new NullReferenceException("В БД не найдено ни одной роли. Создайте её вручную");
                }
                //newUser.Role = _mapper.Map<Role>(userRole);
                result = await _userManager.AddToRoleAsync(newUser, userRole.Name);
                if (result.Succeeded)
                {
                    resultModel.IsSuccessed = true;
                }
                else
                {
                    resultModel.IsSuccessed = false;
                }

            }
            else
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
            //var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            user.Convert(model);

            var resultModel = new ResultModel(false);

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                resultModel.IsSuccessed = true;
                resultModel.AddMessage("Updated successfully");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    resultModel.AddMessage(error.Description);
                }
            }

            return resultModel;
        }

        public async void DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            repository.Delete(user);
        }
    }

}
