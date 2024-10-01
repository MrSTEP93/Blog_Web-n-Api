using Microsoft.AspNetCore.Identity;

namespace FinalBlog.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
