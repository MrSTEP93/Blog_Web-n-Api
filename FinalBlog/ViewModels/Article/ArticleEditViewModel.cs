using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleEditViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок не может быть пустым!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержимое не может быть пустым!")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string AuthorId { get; set; }

        public AuthorViewModel? Author { get; set; }

        public DateTime CreationTime { get; private set; } = DateTime.Now;
    }
}
