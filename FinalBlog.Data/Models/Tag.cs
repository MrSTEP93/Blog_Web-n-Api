namespace FinalBlog.Data.Models
{
    public class Tag(string name)
    {
        public int Id { get; set; }

        public string Name { get; set; } = name;

        public List<Article>? Articles { get; set; }
    }
}
