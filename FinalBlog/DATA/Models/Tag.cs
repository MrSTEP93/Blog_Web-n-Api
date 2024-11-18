namespace FinalBlog.DATA.Models
{
    public class Tag(string name)
    {
        public int Id { get; set; }

        public string Name { get; set; } = name;
    }
}
