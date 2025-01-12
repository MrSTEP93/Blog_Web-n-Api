using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Comment
{
    public class CommentAddViewModel
    {
        [Required(ErrorMessage = "Поле ArticleId не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение ArticleId должно быть больше {1}")]
        [Display(Name = "ID статьи", Prompt = "ID статьи (должен заполниться автоматически)")]
        public int ArticleId { get; set; }

        public ArticleViewModel? Article { get; set; }
        
        [Required(ErrorMessage = "Id автора не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "ID автора (должен заполниться автоматически)")]
        public string AuthorId { get; set; }

        public AuthorViewModel? Author { get; set; }

        [Required(ErrorMessage = "Комментарий должен содержать текст!")]
        [Display(Name = "Текст комментария", Prompt = "Введите текст комментария")]
        public string Text { get; set; }

        [Display(Name = "Дата создания комментария", Prompt = "Дата создания комментария (должна заполниться автоматически)")]
        public virtual DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
