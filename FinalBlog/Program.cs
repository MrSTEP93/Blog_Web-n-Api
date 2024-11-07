using AutoMapper;
using FinalBlog.DATA;
using FinalBlog.DATA.Models;
using FinalBlog.DATA.Repositories;
using FinalBlog.Extensions;
using FinalBlog.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "bin\\Debug\\net8.0", "appsettings.json"))
                .AddJsonFile(Path.Combine(Environment.CurrentDirectory, "bin\\Debug\\net8.0", "appsettings.Development.json"));
            string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

            if (string.IsNullOrEmpty(connection))
            {
                throw new Exception("Can't get connection string from \"appsettings<Development>\" file");
            }

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services
                .AddDbContext<AppDbContext>(options => options.UseSqlServer(connection))
                .AddIdentity<BlogUser, Role>(opts => {
                    opts.Password.RequiredLength = 3;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireUppercase = false;
                    opts.Password.RequireDigit = false;
                })
                .AddRoles<Role>()
                .AddEntityFrameworkStores<AppDbContext>();

            var mapperConfig = new MapperConfiguration((c) =>
            {
                c.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services
                .AddSingleton(mapper)
                .AddTransient<IUserService, UserService>()
                .AddTransient<IRoleService, RoleService>();

            builder.Services
                .AddUnitOfWork()
                .AddCustomRepository<BlogUser, UserRepository>()
                .AddCustomRepository<Role, RoleRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
