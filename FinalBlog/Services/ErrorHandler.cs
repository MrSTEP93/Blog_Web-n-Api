using FinalBlog.Data.Models;
using FinalBlog.Services.Interfaces;

namespace FinalBlog.Services
{
    /// <summary>
    /// Глобальный обработчик ошибок
    /// </summary>
    public class ErrorHandler<T>(ILogger<T> logger) : IErrorHandler where T: class
    {
        readonly ILogger<T> _logger = logger;

        /// <summary>
        /// Метод обрабатывает полученный извне Exception, записывает результат в ILogger<T>
        /// </summary>
        /// <param name="logMessage">Сообщение для записи в лог</param>
        /// <param name="ex">полученное исключение </param>
        /// <returns>Возвращает ResultModel</returns>
        public ResultModel ProcessException(string logMessage, Exception ex)
        {
            var resultModel = new ResultModel();
            resultModel.MarkAsFailed(ex.Message);
            _logger.LogError($"{logMessage}: {ex.Message}");
            if (ex.InnerException is not null)
            {
                _logger.LogError($" --- InnerException: {ex.InnerException.Message}");
                resultModel.AddMessage(ex.InnerException.Message);
            }
            return resultModel;
        }
    }
}
