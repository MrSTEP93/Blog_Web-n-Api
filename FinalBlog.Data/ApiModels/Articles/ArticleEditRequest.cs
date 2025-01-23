using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalBlog.Data.ApiModels.Articles
{
    /// <summary>
    /// Модель редактирования статьи для API
    /// </summary>
    public class ArticleEditRequest : ArticleAddRequest
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "служебное поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        /// <summary>
        /// Дата создания статьи (заполняется автоматически)
        /// </summary>
        public override DateTime CreationTime { get; set; }

        /// <summary>
        /// Перечень тегов статьи
        /// </summary>
        //public List<TagResponse> Tags { get; set; } = [];


    }
}
