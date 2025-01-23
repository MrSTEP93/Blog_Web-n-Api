using System.ComponentModel.DataAnnotations;

namespace FinalBlog.Data.ApiModels.Comment
{
    /// <summary>
    /// Модель создания комментария для API
    /// </summary>
    public class CommentAddRequest
    {
        [Required(ErrorMessage = "Поле ArticleId не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение ArticleId должно быть больше {1}")]
        [Display(Name = "ID статьи", Prompt = "ID статьи (должен заполниться автоматически)")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "Id автора не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "ID автора (должен заполниться автоматически)")]
        public string AuthorId { get; set; }

        [Required(ErrorMessage = "Комментарий должен содержать текст!")]
        [Display(Name = "Текст комментария", Prompt = "Введите текст комментария")]
        public string Text { get; set; }

        [Display(Name = "Дата создания комментария", Prompt = "Дата создания комментария (должна заполниться автоматически)")]
        public virtual DateTime CreationTime { get; set; }
    }
}
