using AutoMapper;
using FinalBlog.Controllers;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.Extensions;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NLog;

namespace FinalBlog.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<BlogUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        readonly ILogger<RoleService> _logger;

        public RoleService(
            UserManager<BlogUser> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<RoleService> logger
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;

            var repository = _unitOfWork.GetRepository<Role>() as RoleRepository;
            if (!repository.GetAll().Any())
            {
                CreateBaseRoles();
            }
        }

        /// <summary>
        /// Служебный метод для создания стартовых ролей
        /// </summary>
        private void CreateBaseRoles()
        {
            RoleAddViewModel userRoleModel = new()
            {
                Name = "Пользователь",
                Description = "Пользователь сайта"
            };
            AddRole(userRoleModel);

            RoleAddViewModel adminRoleModel = new()
            {
                Name = "Администратор",
                Description = "Администратор сайта"
            };
            AddRole(adminRoleModel);

            RoleAddViewModel moderatorRoleModel = new()
            {
                Name = "Модератор",
                Description = "Имеет право редактировать статьи"
            };
            AddRole(moderatorRoleModel);
        }

        public async Task<ResultModel> AddRole(RoleAddViewModel model)
        {
            Role newRole = new()
            {
                Name = model.Name,
                Description = model.Description
            };
            var result = await _roleManager.CreateAsync(newRole);
            ResultModel resultModel = new(in result);
            if (resultModel.IsSuccessed)
                _logger.LogInformation($"Создана роль \"{model.Name}\" id={newRole.Id}");
            else
                _logger.LogInformation($"Ошибка при создании роли \"{model.Name}\" id={newRole.Id}: {resultModel.Messages[0]}");

            return resultModel;
        }

        public async Task<ResultModel> UpdateRole(RoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return new ResultModel(false, "Роль с таким Id не обнаружена!");

            ResultModel resultModel = new(true, "Роль успешно обновлена");
            try
            {
                role.ConvertRole(model);
                var result = await _roleManager.UpdateAsync(role);
                _logger.LogInformation($"Обновлена роль id={model.Id}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка обновления роли id={model.Id}", ex);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                return new ResultModel(false, "Роль с таким Id не обнаружена!");
            
            var result = await _roleManager.DeleteAsync(role);
            ResultModel resultModel = new(in result, "Роль удалена");

            if (resultModel.IsSuccessed)
                _logger.LogInformation($"Удалена роль id={roleId}");
            else
                _logger.LogInformation($"Ошибка при удалении роли id={roleId}: {resultModel.Messages[0]}");
            return resultModel;
        }

        public List<RoleViewModel> GetAllRoles()
        {
            var repo = _unitOfWork.GetRepository<Role>() as RoleRepository;
            var roles = repo.GetAll();
            var rolesView = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleViewModel>(role));
            }
            return rolesView;
        }

        public RoleViewModel GetRoleById(string roleId)
        {
            var role = _roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }
        
        public RoleViewModel GetRoleByName(string roleName)
        {
            var role = _roleManager.Roles.Where(r => r.Name == roleName).FirstOrDefault();
            RoleViewModel model = new();
            model = _mapper.Map<RoleViewModel>(role);
            return model;
        }

        public async Task<List<Role>> GetRolesOfUser(BlogUser user)
        {
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.ToListAsync();
            var result = new List<Role>();
            foreach (var item in allRoles)
            {
                if (userRoleNames.Contains(item.Name!))
                    result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// Глобальный обработчик ошибок (если можно так выразиться xD)
        /// глобальный для этого класса (иначе источник события будет криво отображаться)
        /// </summary>
        /// <param name="logMessage">Сообщение для записи в лог</param>
        /// <param name="ex">Полученнное исключение</param>
        /// <returns></returns>
        private ResultModel ProcessException(string logMessage, Exception ex)
        {
            var resultModel = new ResultModel();
            resultModel.MarkAsFailed(ex.Message);
            _logger.LogError($"{logMessage}: {ex.Message}");
            if (ex.InnerException is not null)
            {
                _logger.LogError($" --- InnerException: {ex.InnerException.Message}");
                resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }
    }
}
