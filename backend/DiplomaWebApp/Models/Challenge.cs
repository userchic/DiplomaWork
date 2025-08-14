namespace DiplomaWebApp.Models
{
    public class Challenge
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public DateTime DeclareTime { get; set; }
        public int TaskId { get; set; }
        public int RequestingTeamId { get; set; }
        public bool IsCheckingCorrectness { get; set; } = false;
        public bool IsChallengeAccepted { get; set; } = false;
        public Round Round { get; set; }
        public Game Game { get; set; }
        public Problem Task { get; set; }
        public Team RequestingTeam { get; set; }
    }
}
