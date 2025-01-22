using Microsoft.AspNetCore.Identity;

namespace FinalBlog.Data.Models
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
