using AutoMapper;
using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.Article;
using FinalBlog.ViewModels.User;

namespace FinalBlog.Extensions
{
    public static class CreateModelFromEntity
    {
        public static UserViewModel ToViewModel(this BlogUser user, IMapper mapper)
        {
            return mapper.Map<UserViewModel>(user);
        }

        public static List<UserViewModel> ToViewModel(this List<BlogUser> userList, IMapper mapper)
        {
            List<UserViewModel> list = [];
            foreach (BlogUser user in userList)
            {
                list.Add(mapper.Map<UserViewModel>(user));
            }
            return list;
        }

        public static ArticleViewModel ToViewModel(this Article article, IMapper mapper)
        {
            return mapper.Map<ArticleViewModel>(article);
        }
        
    }
}
