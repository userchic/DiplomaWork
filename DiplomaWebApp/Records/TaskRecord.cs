using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public record class TaskRecord
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Текст не введен, введите его")]
        [StringLength(500, ErrorMessage = "Текст задачи может вмещать до 500 символов, как вы превысили это ограничение???")]
        public string Text { get; set; }
    }
}
