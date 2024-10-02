using Microsoft.AspNetCore.Identity;

namespace FinalBlog.DATA.Models
{
    public class BlogUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string About { get; set; }

        public Role? Role { get; set; }

        public DateTime RegDate { get; set; }

        public BlogUser()
        {
            FirstName = "undef";
            LastName = "undef";
            About = "Новый пользователь";
            RegDate = DateTime.Now;
        }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
