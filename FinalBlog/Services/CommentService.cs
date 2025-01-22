using AutoMapper;
using FinalBlog.Data.Models;
using FinalBlog.Data.Repositories;
using FinalBlog.Data.UoW;
using FinalBlog.Services.Interfaces;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Tag;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace FinalBlog.Services
{
    public class CommentService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        //IUserService userService,
        ILogger<CommentService> logger
        ) : ICommentService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        //readonly IUserService _userService = userService;
        readonly ILogger<CommentService> _logger = logger;

        public async Task<ResultModel> AddComment(CommentAddViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var resultModel = new ResultModel(true, "Comment created");
            try
            {
                await repo.Create(_mapper.Map<Comment>(model));
                _logger.LogInformation($"К статье id={model.ArticleId} добавлен комментарий автором {model.AuthorId}");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка обновления комментария к статье id={model.ArticleId}", ex);
            }
            return resultModel;
        }

        public async Task<ResultModel> UpdateComment(CommentEditViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var resultModel = new ResultModel(true, "Comment updated");
            try
            {
                await repo.Update(_mapper.Map<Comment>(model));

                _logger.LogInformation($"Комментарий id={model.Id} обновлен");
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка обновления комментария id={model.Id}", ex);
            }
            return resultModel;
        }

        public async Task<ResultModel> DeleteComment(int commentId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = await repo.Get(commentId);
            var resultModel = new ResultModel(true, "Comment deleted");
            try
            {
                await repo.Delete(comment);
            }
            catch (Exception ex)
            {
                resultModel = ProcessException($"Ошибка удаления комментария id={commentId}", ex);
            }
            return resultModel;
        }

        public List<CommentViewModel> GetAllComments()
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var commentList = repo.GetAll().ToList();
            return CreateViewModelList(commentList);
        }

        public async Task<CommentViewModel> GetCommentById(int commentId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = await repo.Get(commentId);
            return _mapper.Map<CommentViewModel>(comment);
        }

        public List<CommentViewModel> GetCommentsOfArticle(int articleId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetAll()
                .Where(x => x.ArticleId == articleId)
                .OrderBy(x => x.CreationTime)
                .ToList();
            var model = CreateViewModelList(comments);
            return model;
        }

        public List<CommentViewModel> GetCommentsOfAuthor(string authorId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = repo.GetAll()
                .Where(x => x.AuthorId == authorId)
                .ToList();
            var model = CreateViewModelList(comments);
            return model;
        }

        public ResultModel CheckIfUserCanEdit(ClaimsPrincipal user, string authorId)
        {
            var resultModel = new ResultModel(false, "Вы не можете редактировать этот комментарий");
            if (
                    user.FindFirstValue(ClaimTypes.NameIdentifier) == authorId
                    || user.IsInRole("Администратор")
                    || user.IsInRole("Модератор")
                )
                resultModel.MarkAsSuccess("Редактирование комментария разрешено");
            else
                _logger.LogError($"$Пользователю id={user.FindFirstValue(ClaimTypes.Email)} запрещено редактировать комментарий");

            return resultModel;
        }

        public ResultModel CheckIfUserCanAdd(ClaimsPrincipal user)
        {
            var resultModel = new ResultModel(false, "Вы не можете писать комментарии");
            if (
                    user.IsInRole("Администратор") 
                    || user.IsInRole("Модератор") 
                    || user.IsInRole("Пользователь")
               )
               resultModel.MarkAsSuccess("Добавление комментов разрешено");
            else
                _logger.LogError($"$Пользователю id={user.FindFirstValue(ClaimTypes.Email)} запрещено добавлять комментарий");

            return resultModel;
        }

        private List<CommentViewModel> CreateViewModelList(List<Comment> list)
        {
            var model = new List<CommentViewModel>();
            foreach (var entity in list)
                model.Add(_mapper.Map<CommentViewModel>(entity));

            return model;
        }

        /// <summary>
        /// Глобальный обработчик ошибок (если можно так выразиться xD)
        /// глобальный для этого класса (иначе источник события будет криво отображаться)
        /// </summary>
        /// <param name="logMessage">Сообщение для записи в лог</param>
        /// <param name="ex">полученнное исключение </param>
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
