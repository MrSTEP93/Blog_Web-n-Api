using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Role;
using FinalBlog.ViewModels.User;

namespace FinalBlog.Extensions
{
    public static class UpdateEntityFromModel
    {
        public static BlogUser ConvertUser(this BlogUser user, UserEditViewModel model)
        {
            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.About = model.About;

            return user;
        }

        public static Role ConvertRole(this Role role, RoleViewModel model)
        {
            role.Name = model.Name;
            role.Description = model.Description;

            return role;
        }

        public static Article ConvertArticle(this Article article, ArticleViewModel model)
        {
            article.Id= model.Id;
            article.Title = model.Title;
            article.Content = model.Content;
            article.AuthorID = model.AuthorID;
            article.CreationTime = model.CreationTime;
            
            return article;
        }
    }
}
