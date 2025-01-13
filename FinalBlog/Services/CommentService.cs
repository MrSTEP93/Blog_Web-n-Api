using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
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
        IUnitOfWork unitOfWork
        ) : ICommentService
    {
        readonly IMapper _mapper = mapper;
        readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResultModel> AddComment(CommentAddViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var resultModel = new ResultModel(true, "Comment created");
            try
            {
                await repo.Create(_mapper.Map<Comment>(model));
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
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
            }
            catch (Exception ex)
            {
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
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
                resultModel.MarkAsFailed();
                resultModel.AddMessage(ex.Message);
                if (ex.InnerException is not null)
                    resultModel.AddMessage(ex.InnerException.Message);
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

            return resultModel;
        }

        private List<CommentViewModel> CreateViewModelList(List<Comment> list)
        {
            var model = new List<CommentViewModel>();
            foreach (var entity in list)
                model.Add(_mapper.Map<CommentViewModel>(entity));

            return model;
        }
    }
}
