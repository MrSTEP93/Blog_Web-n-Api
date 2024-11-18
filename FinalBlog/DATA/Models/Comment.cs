namespace FinalBlog.DATA.Models
{
    public class Comment
    {
        public Comment()
        {
            
        }
        public Comment(Article article, BlogUser commentAuthor, string text)
        {
            ArticleId = article.Id;
            AuthorId = commentAuthor.Id;
            Text = text;
        }

        public int Id { get; set; }

        public int ArticleId { get; set; }

        public Article? TargetArticle { get; set; }

        public string AuthorId { get; set; }

        public BlogUser? CommentAuthor { get; set; }

        public string Text { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.Now;
    }
}
