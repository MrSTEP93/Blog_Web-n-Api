namespace FinalBlog.DATA.Models
{
    public class Article
    {
        public Article()
        {

        }

        public Article(string title, string content, BlogUser author)
        {
            Title = title;
            Content = content;
            Author = author;
            AuthorID = author.Id;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorID { get; set; }

        public BlogUser Author { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
