using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleAddViewModel
    {
        [Required(ErrorMessage = "Заголовок не может быть пустым!")]
        [Display(Name = "Заголовок", Prompt = "Введите заголовок статьи")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержимое не может быть пустым!")]
        [Display(Name = "Содержимое", Prompt = "Текст статьи")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "служебное поле")]
        public string AuthorId { get; set; }

        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
