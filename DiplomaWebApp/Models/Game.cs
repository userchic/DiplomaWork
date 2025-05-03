namespace DiplomaWebApp.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int SolvingTime { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public int AssessorPoints { get; set; }
        public string CaptainsRoundFormat { get; set; } = "";
        public ICollection<Problem> Tasks { get; set; }
        public ICollection<Jure> Assessors { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }

        
    }
}
