using System.ComponentModel.DataAnnotations;

namespace FinalBlog.ViewModels.User
{
    public class AuthorViewModel
    {
        public string Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        [Display(Name = "Имя автора")]
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
