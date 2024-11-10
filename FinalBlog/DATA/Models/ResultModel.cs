using Microsoft.AspNetCore.Identity;

namespace FinalBlog.DATA.Models
{
    public class ResultModel
    {
        public bool IsSuccessed { get; set; }

        public List<string> Messages { get; private set; } = [];

        public ResultModel(bool isSuccessed = false, string message = "Operation not completed")
        {
            IsSuccessed = isSuccessed;
            Messages.Add(message);
        }
        
        public ResultModel(in IdentityResult result, string successMessage = "Success")
        {
            ProcessResult(in result, successMessage);
        }

        public void AddMessage(string message)
        {
            Messages.Add(message);
        }
        
        public void MarkAsSuccess(string newMessage = "Success")
        {
            Messages.Clear();
            Messages.Add(newMessage);
            IsSuccessed = true;
        }

        public void FillMessagesFromResult(in IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                AddMessage(error.Description);
            }
        }

        public void ProcessResult(in IdentityResult result, string successMessage = "Success")
        {
            if (result.Succeeded)
                MarkAsSuccess(successMessage);
            else
                FillMessagesFromResult(result);
        }
    }
}
