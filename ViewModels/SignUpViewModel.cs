using System.ComponentModel.DataAnnotations;

namespace NoteTakingApp.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı alanı boş bırakılamaz.")] 
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email formatı yanlıştır.")]
        [Required(ErrorMessage ="Email alanı boş bırakılamaz.")]
        public string Email { get; set; }
       

        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        public string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage = "Şifreler birbiriyle uyuşmuyor.")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz.")]
        public string PasswordConfirm { get; set; }

    }
}
