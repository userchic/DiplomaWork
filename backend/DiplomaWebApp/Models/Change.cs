namespace DiplomaWebApp.Models
{
    public class Change
    {
        public int Id { get; set; }
        public DateTime DeclareTime { get; set; }
        public int RoundId { get; set; }
        public int RoleId { get; set; }
        public int NewParticipantId { get; set; }
        public int InitiatorTeamId { get; set; }
        public Round Round { get; set; }
        public Role Role { get; set; }
        public Student NewParticipant { get; set; }
        public Team InitiatorTeam { get; set; }
    }
}
