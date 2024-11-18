using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Comment
{
    public class CommentAddViewModel
    {
        [Required(ErrorMessage = "Поле ArticleId не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение ArticleId должно быть больше {1}")]
        public int ArticleId { get; set; }
        
        /// <summary>
        /// Временный атрибут Required
        /// </summary>
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string AuthorId { get; set; }

        [Required(ErrorMessage = "Комментарий должен содержать текст!")]
        public string Text { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
