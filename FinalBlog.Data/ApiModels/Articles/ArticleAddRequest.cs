using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBlog.Data.ApiModels.Articles
{
    /// <summary>
    /// Модель добавления статьи для API
    /// </summary>
    public class ArticleAddRequest
    {
        [Required(ErrorMessage = "Заголовок не может быть пустым!")]
        [Display(Name = "Заголовок", Prompt = "Введите заголовок статьи")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Содержимое не может быть пустым!")]
        [Display(Name = "Содержимое", Prompt = "Текст статьи")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "служебное поле (должно заполниться автоматически)")]
        public string AuthorId { get; set; }

        public virtual DateTime CreationTime { get; set; } = DateTime.Now;

    }
}
