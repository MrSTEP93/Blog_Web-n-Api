using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinalBlog.DATA.Models
{
    public class ResultModel
    {
        public bool IsSuccessed { get; set; }

        public List<string> Messages { get; private set; } = [];

        public ResultModel(bool isSuccessed = false)
        {
            IsSuccessed = isSuccessed;
        }

        public ResultModel(bool isSuccessed, string message)
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
        
        public void MarkAsSuccess(string successMessage = "Success")
        {
            Messages.Clear();
            Messages.Add(successMessage);
            IsSuccessed = true;
        }
        
        public void MarkAsFailed(string errorMessage = "Operation not completed")
        {
            Messages.Clear();
            Messages.Add(errorMessage);
            IsSuccessed = false;
        }

        public void ProcessResult(in IdentityResult result, string successMessage = "Success")
        {
            if (result.Succeeded)
                MarkAsSuccess(successMessage);
            else
                FillMessagesFromResult(result);
        }

        public void FillMessagesFromResult(in IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                AddMessage(error.Description);
            }
        }
    }
}
