namespace FinalBlog.ViewModels.Comment
{
    public class CommentListViewModel
    {
        public List<CommentViewModel> CommentList { get; set; } = [];

        public int CommentsCount { get; set; }

        public string? AuthorFullName { get; set; }
    }
}
