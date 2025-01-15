using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Tag
{
    public class TagEditViewModel : TagAddViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        [Display(Name="Статей по тегу: ")]
        public int ArticleCount { get; set; }
    }
}
