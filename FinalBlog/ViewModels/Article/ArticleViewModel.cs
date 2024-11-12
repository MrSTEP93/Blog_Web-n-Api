using FinalBlog.DATA.Models;

namespace FinalBlog.ViewModels.Article
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public BlogUser? Author { get; set; }

        public DateTime CreationTime { get; private set; } = DateTime.Now;
    }
}
