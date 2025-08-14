namespace DiplomaWebApp.Models
{
    public class Round
    {
        public int Id { get; set; }
        public int RoundNumber { get; set; }
        public int? SpeakerId { get; set; }
        public int? OpponentId { get; set; }
        public DateTime StartTime { get; set; }
        public bool NoSolution {  get; set; }
        public int ChallengeId { get; set; }
        public Student Speaker { get; set; }
        public Student Opponent { get; set; }
        public Challenge Challenge { get; set; }
        public RolesChange? RolesChange { get; set; }
        public RoundResults? RoundResults { get; set; }
        public ICollection<Change> Changes { get; set; }
        public ICollection<Break> Breaks { get; set; }
    }
}
