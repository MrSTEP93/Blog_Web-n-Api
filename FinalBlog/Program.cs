using AutoMapper;
using FinalBlog.Data;
using FinalBlog.Data.Models;
using FinalBlog.Data.Repositories;
using FinalBlog.Data.Repositories.Interfaces;
using FinalBlog.Extensions;
using FinalBlog.Services;
using FinalBlog.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog.Extensions.Logging;

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
                throw new Exception("Can't get connection string from the file \"appsettings.Development.json\"");
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
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IArticleService, ArticleService>()
                .AddTransient<ITagService, TagService>()
                .AddTransient<ICommentService, CommentService>();

            builder.Services
                .AddTransient<IArticleRepository, ArticleRepository>()
                .AddTransient<ITagRepository, TagRepository>();

            builder.Services
                .AddUnitOfWork()
                .AddCustomRepository<BlogUser, UserRepository>()
                .AddCustomRepository<Role, RoleRepository>()
                .AddCustomRepository<Comment, CommentRepository>();

            // Connect logger
            builder.Logging
                .ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole()
                .AddNLog("nlog");

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

            app.UseStatusCodePages(async statusCodeContext =>
            {
                var response = statusCodeContext.HttpContext.Response;

                response.ContentType = "text/plain; charset=UTF-8";
                if (response.StatusCode == 400)
                    response.Redirect("/BadRequest");

                else if (response.StatusCode == 404)
                    response.Redirect("/NotFound");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
