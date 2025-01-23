using System.ComponentModel.DataAnnotations;

namespace FinalBlog.Data.ApiModels.Comment
{
    /// <summary>
    /// Модель редактирования комментария для API
    /// </summary>
    public class CommentEditRequest : CommentAddRequest
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }
    }
}
