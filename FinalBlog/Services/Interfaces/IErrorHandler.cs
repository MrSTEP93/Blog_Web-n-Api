using FinalBlog.Data.Models;

namespace FinalBlog.Services.Interfaces
{
    public interface IErrorHandler
    {
        ResultModel ProcessException(string logMessage, Exception ex);
    }
}
