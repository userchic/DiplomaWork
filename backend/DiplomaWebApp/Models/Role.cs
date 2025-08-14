namespace DiplomaWebApp.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Change> Changes { get; set; }
    }
}
