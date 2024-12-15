using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Role
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Нужно заполнить Имя тега!")]
        public string Name { get; set; }

        public string? Description { get; set; }
    }
}
