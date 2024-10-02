using FinalBlog.DATA.Models;
using FinalBlog.ViewModels.User;

namespace FinalBlog.Extensions
{
    public static class UpdateUserFromModel
    {
        public static BlogUser Convert(this BlogUser user, UserEditViewModel model)
        {
            //user.PathToPhoto = usereditvm.PathToPhoto;
            user.LastName = model.LastName;
            user.FirstName = model.FirstName;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.About = model.About;

            return user;
        }
    }
}
