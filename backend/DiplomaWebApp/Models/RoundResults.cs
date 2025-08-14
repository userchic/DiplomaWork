namespace DiplomaWebApp.Models
{
    public class RoundResults
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public DateTime RoundEndTime { get; set; }
        public int Team1Points { get; set; } 
        public int Team2Points { get; set; }
        public bool Correctness { get; set; }
        public Round Round { get; set; }
        public ICollection<Mistake> Mistakes { get; set; }
    }
}
