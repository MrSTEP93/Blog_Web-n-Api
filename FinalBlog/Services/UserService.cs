using AutoMapper;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                resultModel.MarkAsSuccess("Успешная авторизация");
            else
            {
                resultModel.AddMessage("Неправильный логин и (или) пароль");
            }

            return resultModel;
        }

        public async Task Logout() => await _signInManager.SignOutAsync();

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            var userList = repository.GetAll().ToList();

            var model = new List<UserViewModel>();
            foreach (var user in userList)
            {
                user.Roles = await _roleService.GetRolesOfUser(user);
                model.Add(_mapper.Map<UserViewModel>(user));
            }

            return model;
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            var user = await repository.Get(id)
                ?? throw new NullReferenceException($"Пользователь не найден в базе (id={id})");
            user.Roles = await _roleService.GetRolesOfUser(user);
            var model = _mapper.Map<UserViewModel>(user);
            //model.Roles = userRoles;

            return model;
        }

        public async Task<UserViewModel> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal)
                ?? throw new NullReferenceException($"Пользователь не найден в базе данных");
            user.Roles = await _roleService.GetRolesOfUser(user);
            var model = _mapper.Map<UserViewModel>(user);

            return model;
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
            return resultModel;
        }

        public async Task<ResultModel> CreateRandomUsers()
        {
            byte userCount = 5;
            var usergen = new UserGenerator();
            var userlist = usergen.Populate(userCount);
            var resultModel = new ResultModel(false);

            foreach (var user in userlist)
            {
                var result = await _userManager.CreateAsync(user, "123456");

                if (!result.Succeeded)
                {
                    resultModel.ProcessResult(result);
                }
            }

            return resultModel;
        }
    }
}
