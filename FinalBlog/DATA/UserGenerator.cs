using FinalBlog.DATA.Models;

namespace FinalBlog.DATA
{
    public class UserGenerator
    {
        public readonly string[] maleNames = ["Алексей", "Борян", "Василий", "Игорь", "Даниил", "Сергей", "Евгений", "Григорий", "Витек", "Миха"];
        public readonly string[] femaleNames = ["Анна", "Мария", "Станислава", "Елена", "Юлия", "Настя"];
        public readonly string[] lastNames = [ "Тестов", "Туголуков", "Потапов", "Шкуров", "Лысенков" ];

        public List<BlogUser> Populate(int count)
        {
            var users = new List<BlogUser>();
            for (int i = 1; i <= count; i++)
            {
                string firstName;
                var rand = new Random();

                var male = rand.Next(1, 2) == 1;

                var lastName = lastNames[rand.Next(0, lastNames.Length - 1)];
                if (male)
                {
                    firstName = maleNames[rand.Next(0, maleNames.Length - 1)];
                }
                else
                {
                    lastName += "a";
                    firstName = femaleNames[rand.Next(0, femaleNames.Length - 1)];
                }

                var item = new BlogUser()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = "test" + rand.Next(0, 1204) + "@test.com",
                };

                item.UserName = item.Email;

                users.Add(item);
            }

            return users;
        }
    }
}
