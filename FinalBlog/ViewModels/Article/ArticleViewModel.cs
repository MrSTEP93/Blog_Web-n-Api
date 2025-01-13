using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleViewModel : ArticleEditViewModel
    {
        public List<CommentViewModel> Comments { get; set; } = [];

        public int CommentsCount { 
                get { return Comments.Count; }
            }

        private CommentAddViewModel _commentAddViewModel = new();

        public CommentAddViewModel CommentAddViewModel {
            get 
            {
                _commentAddViewModel.ArticleId = Id;
                return _commentAddViewModel; 
            }
            set { }
        } 
    }
}
