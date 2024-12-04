using AutoMapper;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
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
            const string defaultRoleName = "Пользователь0";
            ResultModel resultModel = new(false);
            var newUser = _mapper.Map<BlogUser>(model);

            var result = await _userManager.CreateAsync(newUser, model.RegPassword);
            if (result.Succeeded)
            {
                resultModel.AddMessage("Пользователь зарегистрирован");
                await _signInManager.SignInAsync(newUser, false);
                try
                {
                    result = await _userManager.AddToRoleAsync(newUser, defaultRoleName);
                    resultModel.AddMessage($"Присвоена роль по умолчанию ({defaultRoleName})");
                    resultModel.IsSuccessed = true;
                }
                catch (Exception ex)
                {
                    resultModel.AddMessage($"Не удалось присвоить пользователю роль по умолчанию ({defaultRoleName})");
                    resultModel.IsSuccessed = false;
                    resultModel.AddMessage(ex.Message);
                    if (ex.InnerException != null)
                        resultModel.AddMessage(ex.InnerException.Message);
                }
            } else
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
                resultModel.AddMessage("Неправильный пароль");
            }

            return resultModel;
        }

        public async Task<ResultModel> LoginWithClaims(LoginViewModel model)
        {
            ResultModel resultModel = new(false);
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == model.Email)
                ?? throw new Exception("Email was not found!");
            user.Roles = await _roleService.GetRolesOfUser(user);
            var result = await _signInManager.CheckPasswordSignInAsync(user!, model.Password, false);
            if (result.Succeeded)
            {
                var claims = GetUserClaims(user);
                await _signInManager.SignInWithClaimsAsync(user!, false, claims);
                resultModel.MarkAsSuccess("Успешная авторизация");
            }
            else
            {
                resultModel.AddMessage("Неправильный пароль");
            }

            return resultModel;
        }

        public async Task Logout() => await _signInManager.SignOutAsync();

        public async Task<List<BlogUser>> GetAllUsers()
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            var userList = repository.GetAll().ToList();

            //var model = new List<UserViewModel>();
            List<BlogUser> list = [];
            foreach (var user in userList)
            {
                user.Roles = await _roleService.GetRolesOfUser(user);
                //model.Add(_mapper.Map<UserViewModel>(user));
            }

            return list;
        }

        public List<Claim> GetUserClaims(BlogUser user)
        {
            var claims = new List<Claim>();

            foreach (var role in user.Roles)
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Name!));
            claims.Add(new Claim("UserID", user.Id));

            return claims;
        }

        public async Task<BlogUser> GetUserById(string id)
        {
            var repository = _unitOfWork.GetRepository<BlogUser>() as UserRepository;
            var user = await repository.Get(id)
                ?? throw new NullReferenceException($"Пользователь не найден в базе (id={id})");
            user.Roles = await _roleService.GetRolesOfUser(user);
            //var model = _mapper.Map<UserViewModel>(user);
            //model.Roles = userRoles;

            return user;
        }

        public async Task<BlogUser> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal)
                ?? throw new NullReferenceException($"Пользователь не найден в базе данных");
            user.Roles = await _roleService.GetRolesOfUser(user);
            //var model = _mapper.Map<UserViewModel>(user);

            return user;
        }

        public async Task<ResultModel> UpdateUserInfo(UserViewModel model, List<string> newRolesList)
        {
            var resultModel = await UpdateUserInfo(model);
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            var allRoles = _roleService.GetAllRoles();
            foreach (var role in allRoles)
            {
                if (newRolesList.Contains(role.Name))
                {
                    if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        var result = await _userManager.AddToRoleAsync(user, role.Name);
                        string resMessage = $"Роль \"{role.Name}\" " + (result.Succeeded ? "успешно добавлена" : "добавить не удалось" );
                        resultModel.AddMessage(resMessage);
                    }
                }
                else
                {
                    if (await _userManager.IsInRoleAsync(user, role.Name))
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                        string resMessage = $"Роль \"{role.Name}\" " + (result.Succeeded ? "успешно удалена" : "удалить не удалось");
                        resultModel.AddMessage(resMessage);
                    }
                }
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateUserInfo(UserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            user.ConvertUser(model);
            var result = await _userManager.UpdateAsync(user);

            ResultModel resultModel = new(in result, "Данные успешно обновлены");
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
