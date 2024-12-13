using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Data;
using NoteTakingApp.Extensions;
using NoteTakingApp.Services;

namespace NoteTakingApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon"));
            });
            
            builder.Services.AddIdentityWithExt();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.ConfigureApplicationCookie(opt =>
            {
                var cookieBuilder = new CookieBuilder();

                cookieBuilder.Name = "NoteTakingApp";
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.Cookie = cookieBuilder;
                opt.ExpireTimeSpan = TimeSpan.FromDays(7);
                opt.SlidingExpiration = true;
            });
            var configuration = builder.Configuration;

            builder.Services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = configuration["Google:ClientId"];
                googleOptions.ClientSecret = configuration["Google:Secret"];
            });

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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
