using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Tag
{
    public class TagEditViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Нужно заполнить Имя тега!")]
        public string Name { get; set; }
    }
}
