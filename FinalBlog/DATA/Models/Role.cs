using Microsoft.AspNetCore.Identity;

namespace FinalBlog.DATA.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
