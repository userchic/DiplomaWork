using DiplomaWebApp.DataBase;

namespace DiplomaWebApp.Repositories
{
    public abstract class AbstractRepository
    {
        protected MathBattlesDbContext _context;
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
