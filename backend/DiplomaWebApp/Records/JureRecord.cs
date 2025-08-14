using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public class JureRecord
    {
        [Required(ErrorMessage = "Не введено имя пользователя, введите его")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не введена фамилия пользователя, введите её")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Не введено отчество пользователя, введите его")]
        public string Fatname { get; set; }
        [Required(ErrorMessage = "Не введен логин, введите его")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не введен пароль, введите его")]
        public string Password { get; set; }
    }
}
