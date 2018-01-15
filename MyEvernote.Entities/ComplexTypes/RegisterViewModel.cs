using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyEvernote.Entities.ComplexTypes
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
            StringLength(25, ErrorMessage = "{0} max.  {1} karakter olmalıdır !")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), 
            Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
            StringLength(70, ErrorMessage = "{0} max.  {1} karakter olmalıdır !"),
            EmailAddress(ErrorMessage = "{0} alanı için geçerli bir e-posta adresi giriniz !")]
        public string EMail { get; set; }
        [DisplayName("Şifre"),
         Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
         StringLength(25, ErrorMessage = "{0} max.  {1} karakter olmalıdır !")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"),
            Required(ErrorMessage = "{0} alanı boş geçilemez !"), 
            StringLength(25, ErrorMessage = "{0} max.  {1} karakter olmalıdır !"),
            Compare("Password",ErrorMessage = "{0} ve {1} eşleşemedi !")]
        public string RePassword { get; set; }
    }
}