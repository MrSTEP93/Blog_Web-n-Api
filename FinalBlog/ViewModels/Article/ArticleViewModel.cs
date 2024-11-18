using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; }

        public string Title { get; } = string.Empty;

        public string Content { get; } = string.Empty;

        //public string AuthorId { get; set; } = string.Empty;

        public AuthorViewModel? Author { get; }

        public DateTime CreationTime { get; }

        public List<CommentViewModel> Commets { get; } = [];

        public List<TagViewModel> Tags { get; } = [];
    }
}
