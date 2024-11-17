using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Comment
{
    public class CommentEditViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле ArticleId не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение ArticleId должно быть больше {1}")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string AuthorId { get; set; }

        public AuthorViewModel Author { get; set; }

        [Required(ErrorMessage = "Комментарий должен содержать текст!")]
        public string Text { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
