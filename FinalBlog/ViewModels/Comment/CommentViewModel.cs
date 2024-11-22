using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; }

        public int ArticleId { get; }

        //[Required(ErrorMessage = "Поле Id не может быть пустым!")]
        //public string AuthorId { get; set; }

        //public AuthorViewModel? Author { get; set; }

        public AuthorViewModel? Author { get; }

        [Required(ErrorMessage = "Комментарий должен содержать текст!")]
        public string Text { get; } = string.Empty;

        public DateTime CreationTime { get; } 
    }
}
