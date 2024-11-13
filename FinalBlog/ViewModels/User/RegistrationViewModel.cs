using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.User
{
    public class RegistrationViewModel : BaseUserData
    {
        [Required(ErrorMessage = "Поле Пароль обязательно для заполнения")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль", Prompt = "Введите пароль")]
        [StringLength(100, ErrorMessage = "{0} должен содержать от {2} до {1} символов.", MinimumLength = 3)]
        public string RegPassword { get; set; }

        [Required(ErrorMessage = "Обязательно подтвердите пароль")]
        [Compare("RegPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль", Prompt = "Введите пароль еще раз")]
        public string RegPasswordConfirm { get; set; }

        public string? Login => Email;

    }
}
