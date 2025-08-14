using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Records
{
    public record EndRoundRecord
    {
        [Range(0,12,ErrorMessage ="Очки командд и жюри не могут быть отрицательными или больше 12")]
        public int Team1Points { get; set; }
        [Range(0, 12, ErrorMessage = "Очки командд и жюри не могут быть отрицательными или больше 12")]
        public int Team2Points { get; set; }
        [Range(0, 12, ErrorMessage = "Очки командд и жюри не могут быть отрицательными или больше 12")]
        public int AssessorsPoints { get; set; }
        public ICollection<MistakeRecord>? Mistakes { get; set; }
    }
}
