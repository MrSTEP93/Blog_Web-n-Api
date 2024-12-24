using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleViewModel : ArticleEditViewModel
    {
        public List<CommentViewModel> Comments { get; } = [];

        public int CommentsCount { get; } = 0;

        public CommentAddViewModel CommentAddViewModel { get; set; } = new();

        public ArticleViewModel()
        {
            CommentsCount = Comments.Count;
        }
    }
}
