using Microsoft.AspNetCore.Identity;
using NoteTakingApp.CustomValidations;
using NoteTakingApp.Data;
using NoteTakingApp.Localizations;
using NoteTakingApp.Models;

namespace NoteTakingApp.Extensions
{
    public static class StartupExtensions
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromHours(24);
            });

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
            })
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddPasswordValidator<PasswordValidator>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
