namespace FinalBlog.DATA.Models
{
    public class ResultModel
    {
        public bool IsSuccessed { get; set; }

        public List<string> Messages { get; set; } = [];

        public ResultModel(bool isSuccessed, string message = "Operation not completed")
        {
            IsSuccessed = isSuccessed;
            Messages.Add(message);
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
    }
}
