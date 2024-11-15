using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Tag
{
    public class TagAddViewModel
    {
        [Required(ErrorMessage="Нужно заполнить Имя тэга!")]
        public string Name { get; set; }
    }
}
