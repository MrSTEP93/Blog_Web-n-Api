using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.DATA.UoW;
using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Tag;
using System.ComponentModel.Design;

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

        public List<CommentEditViewModel> GetAllComments()
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var commentList = repo.GetAll().ToList();
            return CreateListOfViewModel(commentList);
        }

        public async Task<CommentEditViewModel> GetCommentById(int commentId)
        {
            var repo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comment = await repo.Get(commentId);
            return _mapper.Map<CommentEditViewModel>(comment);
        }

        public List<CommentEditViewModel> GetCommentsOfArticle(int articleId)
        {
            throw new NotImplementedException();
        }

        private List<CommentEditViewModel> CreateListOfViewModel(List<Comment> list)
        {
            var model = new List<CommentEditViewModel>();
            foreach (var entity in list)
            {
                model.Add(_mapper.Map<CommentEditViewModel>(entity));
            }

            return model;
        }
    }
}
