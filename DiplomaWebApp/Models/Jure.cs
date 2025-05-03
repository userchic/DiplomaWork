

namespace DiplomaWebApp.Models
{
    public class Jure
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string FatName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}
