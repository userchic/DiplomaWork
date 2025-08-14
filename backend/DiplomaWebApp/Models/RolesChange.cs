namespace DiplomaWebApp.Models
{
    public class RolesChange
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public DateTime ChangeTime { get; set; }
        public bool FullRoleChange { get; set; }
        public Round Round { get; set; }
    }
}
