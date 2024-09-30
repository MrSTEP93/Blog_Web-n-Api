namespace FinalBlog.Models
{
    public class Comment(Article article, BlogUser commentAuthor, string text)
    {
        public int Id { get; set; }

        public int ArticleId { get; set; } = article.Id;

        public Article? TargetArticle { get; set; }

        public string AuthorId { get; set; } = commentAuthor.Id;

        public BlogUser? CommentAuthor { get; set; }

        public string Text { get; set; } = text;

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
