using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public class LoginRequestRecord
    {
        [Required(ErrorMessage ="Не введен логин, введите его")]
        public string Login { get; set; }
        [Required(ErrorMessage ="Не введен пароль, введите его")]
        public string Password { get; set; }
    }
}
