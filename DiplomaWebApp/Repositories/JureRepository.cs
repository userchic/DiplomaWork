using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class JureRepository : AbstractRepository, IJureRepository
    {
        public void AddJure(Jure newJure)
        {
            _context.Jures.Add(newJure);
            Save();
        }

        public void EditJure(Jure editedJure)
        {
            _context.Jures.Update(editedJure);
            Save();
        }

        public Jure GetJure(string login)
        {
            return _context.Jures.First(x => x.Login == login);
            
        }

        public ICollection<Jure> GetJures()
        {
            return (ICollection<Jure>)_context.Jures.Select(x=>x);
        }

        public void RemoveJure(string login)
        {
            _context.Remove(GetJure(login));
            Save();
        }
    }
}
