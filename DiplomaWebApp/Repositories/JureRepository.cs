using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class JureRepository : Repository, IJureRepository
    {
        public JureRepository(MathBattlesDbContext context) : base(context)
        {
        }

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
            return _context.Jures.FirstOrDefault(x => x.Login == login);
            
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
