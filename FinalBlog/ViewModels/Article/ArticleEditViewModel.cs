using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Tag;
using FinalBlog.ViewModels.User;
using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleEditViewModel : ArticleAddViewModel
    {
        [Required(ErrorMessage = "Поле Id не может быть пустым!")]
        [Display(Name = "ID автора", Prompt = "служебное поле")]
        [Range(1, int.MaxValue, ErrorMessage = "Значение Id должно быть больше {1}")]
        public int Id { get; set; }

        public AuthorViewModel? Author { get; set; }

        public override DateTime CreationTime { get; set; }

        public List<TagViewModel> Tags { get; set; } = [];

        public List<int> TagIds => Tags.Select(t => t.Id).ToList();

        public List<TagViewModel> AllTags { get; set; } = [];

        public List<int> SelectedTagIds { get; set;} = [];
        
    }
}
