namespace DiplomaWebApp.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public ICollection<Game> Games { get; set; }
    }
}
