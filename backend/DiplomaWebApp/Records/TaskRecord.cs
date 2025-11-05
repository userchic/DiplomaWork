using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public record class TaskRecord
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Текст не введен, введите его")]
        public string Text { get; set; }
    }
}
