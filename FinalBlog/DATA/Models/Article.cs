namespace FinalBlog.DATA.Models
{
    public class Article(string title, string content, BlogUser author)
    {
        public int Id { get; set; }

        public string Title { get; set; } = title;

        public string Content { get; set; } = content;

        public string AuthorID { get; set; } = author.Id;

        public BlogUser Author { get; set; } = author;

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
