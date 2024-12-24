using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Role
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Нужно заполнить Имя тега!")]
        [Display(Name = "Имя", Prompt = "Название роли")]
        public string Name { get; set; }

        [Display(Name = "Описание", Prompt = "Описание роли")]
        public string? Description { get; set; }
    }
}
