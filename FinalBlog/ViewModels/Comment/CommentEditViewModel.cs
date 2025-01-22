using FinalBlog.Data.Models;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Comment
{
    public class CommentEditViewModel : CommentAddViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        //public override DateTime CreationTime { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
