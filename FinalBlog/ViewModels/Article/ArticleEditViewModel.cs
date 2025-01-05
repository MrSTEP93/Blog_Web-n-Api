using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleEditViewModel : ArticleAddViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        public AuthorViewModel? Author { get; set; }

        public override DateTime CreationTime{ get; set;  }

        public List<TagViewModel> Tags { get; } = [];

        public List<TagViewModel> AllTags { get; } = [];
    }
}
