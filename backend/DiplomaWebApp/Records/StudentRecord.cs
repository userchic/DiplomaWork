using DiplomaWebApp.Models;
using DiplomaWebApp.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public class StudentRecord
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не введено имя студента, введите его")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Не введена фамилия студента, введите её")]
        public string Surname { get; set; }
        public string? Fatname { get; set; }

        [IsEmailOrEmptyString(ErrorMessage="Введенный Email не распознан как EMail. Введите еще раз")]
        public string? Email { get; set; }
    }
}
