using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Comment;

namespace FinalBlog.Services.Interfaces
{
    public interface ICommentService
    {
        public Task<ResultModel> AddComment(CommentAddViewModel model);
        public Task<ResultModel> UpdateComment(CommentEditViewModel model);
        public Task<ResultModel> DeleteComment(int tagId);
        public Task<CommentViewModel> GetCommentById(int commentId);
        public List<CommentViewModel> GetAllComments();
        public List<CommentViewModel> GetCommentsOfArticle(int articleId);
    }
}
