using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
namespace NoteTakingApp.Localizations
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new()
            {
                Code = "DuplicateUserName",
                Description = $"{userName} başka bir kullanıcı tarafından alınmıştır"
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new()
            {
                Code = "DuplicateEmail",
                Description = $"{email} başka bir kullanıcı tarafından alınmıştır"
            };
        }
        public override IdentityError PasswordTooShort(int length)
        {
            return new()
            {
                Code = "PasswordTooShort",
                Description = $"Şifre en az 6 karakterli olmalıdır."
            };
        }
        public override IdentityError PasswordRequiresDigit()
        {
            return new()
            {
                Code = "PasswordRequiresDigit",
                Description = "Şifre en az bir rakam içermelidir."
            };
        }
        public override IdentityError PasswordRequiresUpper()
        {
            return new()
            {
                Code = "PasswordRequiresUpper",
                Description = "Şifre en az bir büyük harf içermelidir."
            };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new()
            {
                Code = "PasswordRequiresLower",
                Description = "Şifre en az bir küçük harf içermelidir."
            };
        }
        public override IdentityError PasswordMismatch()
        {
            return new()
            {
                Code = "PasswordMismatch",
                Description = "Girilen şifreler eşleşmiyor."
            };
        }
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Şifre en az bir özel karakter içermelidir (örneğin: !, @, #, ...)."
            };
        }
    }
}
