using FinalBlog.Data.Models;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleEditViewModel : ArticleAddViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "служебное поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        /// <summary>
        /// Поле хранит ViewModel автора статьи
        /// </summary>
        public AuthorViewModel? Author { get; set; }

        /// <summary>
        /// Дата создания статьи (заполняется автоматически)
        /// </summary>
        public override DateTime CreationTime { get; set; }

        /// <summary>
        /// Перечень тегов статьи
        /// </summary>
        public List<TagViewModel> Tags { get; set; } = [];

        /// <summary>
        /// Перечень идентификаторов тегов статьи
        /// </summary>
        public List<int> TagIds => Tags.Select(t => t.Id).ToList();

        /// <summary>
        /// Перечень всех доступных тегов
        /// </summary>
        public List<TagViewModel> AllTags { get; set; } = [];

        /// <summary>
        /// Перечень выбранных тегов (отмеченных на форме редактирования)
        /// </summary>
        public List<int>? SelectedTagIds { get; set; } = [];
    }
}
