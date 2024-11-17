using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Comment;

namespace FinalBlog.Services
{
    public interface ICommentService
    {
        public Task<ResultModel> AddComment(CommentAddViewModel model);
        public Task<ResultModel> UpdateComment(CommentEditViewModel model);
        public Task<ResultModel> DeleteComment(int tagId);
        public Task<CommentEditViewModel> GetCommentById(int commentId);
        public List<CommentEditViewModel> GetAllComments();
        public List<CommentEditViewModel> GetCommentsOfArticle(int articleId);
    }
}
