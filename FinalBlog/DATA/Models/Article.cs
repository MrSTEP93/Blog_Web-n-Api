namespace FinalBlog.DATA.Models
{
    public class Article
    {
        public Article()
        {

        }

        public Article(string title, string content, string authorId)
        {
            Title = title;
            Content = content;
            AuthorID = authorId;
        }

        public int Id { get; set; }

        public string Title { get; set; } = "no_title";

        public string Content { get; set; } = "no_content";

        public string AuthorID { get; set; }

        public BlogUser? Author { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
