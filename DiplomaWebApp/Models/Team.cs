namespace DiplomaWebApp.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int CaptainId { get; set; }
        public int ViceCaptainId { get; set; }
        public string Name { get; set; }
        public Game Game { get; set; }
        public ICollection<Student> Students { get; set; }
        public Student Captain { get; set; }
        public Student ViceCaptain { get; set; }
    }
}