using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.User
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Необходимо ввести email")]
        [Display(Name = "Email", Prompt = "Ваш логин (email)")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Без пароля никак")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Ваш пароль")]
        [StringLength(100, ErrorMessage = "{0} должен быть от {2} до {1} символов.", MinimumLength = 3)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
