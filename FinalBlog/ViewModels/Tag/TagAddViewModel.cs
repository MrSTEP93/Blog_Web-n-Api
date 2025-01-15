using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Tag
{
    public class TagAddViewModel
    {
        [Required(ErrorMessage="Нужно заполнить Имя тега!")]
        [Display(Name="Имя тега")]
        public string Name { get; set; }
    }
}
