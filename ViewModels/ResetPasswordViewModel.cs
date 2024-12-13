using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Şifreler birbiriyle uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        public string PasswordConfirm { get; set; }
    }
}
