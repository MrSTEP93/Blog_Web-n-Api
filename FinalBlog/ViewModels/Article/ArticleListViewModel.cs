namespace FinalBlog.ViewModels.Article
{
    public class ArticleListViewModel
    {
        public string Title { get; set; } = "Все статьи";
        public List<ArticleViewModel>? Articles { get; set; }
    }
}
