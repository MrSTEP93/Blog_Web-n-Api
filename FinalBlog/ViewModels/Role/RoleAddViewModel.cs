using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Role
{
    public class RoleAddViewModel
    {
        [Required(ErrorMessage = "Нужно заполнить Имя роли!")]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
