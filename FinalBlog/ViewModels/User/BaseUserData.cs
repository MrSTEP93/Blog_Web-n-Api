using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.User
{
    public class BaseUserData
    {
        [Required(ErrorMessage = "Имя обязательно для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Фамилия обязательна для заполнения")]
        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Требуется указать адрес электронной почты")]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Введите ваш email")]
        public string Email { get; set; } = string.Empty;
    }
}
