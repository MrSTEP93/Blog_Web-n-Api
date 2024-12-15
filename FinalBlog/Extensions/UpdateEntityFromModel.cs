using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.Comment;
using FinalBlog.ViewModels.Role;
using FinalBlog.ViewModels.User;

namespace FinalBlog.Extensions
{
    public static class UpdateEntityFromModel
    {
        public static BlogUser ConvertUser(this BlogUser user, UserViewModel model)
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

        public static Article ConvertArticle(this Article article, ArticleEditViewModel model)
        {
            article.Id= model.Id;
            article.Title = model.Title;
            article.Content = model.Content;
            article.AuthorId = model.AuthorId;
            article.CreationTime = model.CreationTime;
            
            return article;
        }

        public static Comment ConvertComment(this Comment comment, CommentEditViewModel model)
        {
            comment.ArticleId = model.ArticleId;
            comment.AuthorId = model.AuthorId;
            comment.Text = model.Text;

            return comment;
        }
    }
}
