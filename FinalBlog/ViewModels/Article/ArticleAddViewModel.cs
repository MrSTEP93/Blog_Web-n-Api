using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleAddViewModel
    {
        [Required(ErrorMessage = "Заголовок не может быть пустым!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержимое не может быть пустым!")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string AuthorId { get; set; }

        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
