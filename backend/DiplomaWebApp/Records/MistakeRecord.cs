using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public class MistakeRecord
    {
        [Required(ErrorMessage ="Не указан текст ошибки, введите ещё раз")]
        public string Text { get; set; }
        public int JureCost { get; set; }
        public int OpponentCost { get; set; }
    }
}
