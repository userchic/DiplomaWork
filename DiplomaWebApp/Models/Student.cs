namespace DiplomaWebApp.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public string Email { get; set; }
        public ICollection<Team> Teams { get; set; }
        public ICollection<Team> TeamsCaptain { get; set; }
        public ICollection<Team> TeamsViceCaptain { get; set; }
    }
}
