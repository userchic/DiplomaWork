namespace DiplomaWebApp.Models
{
    public class Break
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int? InitiatorTeamId { get; set; }
        public Team InitiatorTeam { get; set; }
        public Round Round { get; set; }
        public DateTime DeclareTime { get; set; }
    }
}
