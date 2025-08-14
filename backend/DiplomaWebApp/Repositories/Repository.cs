using DiplomaWebApp.DataBase;

namespace DiplomaWebApp.Repositories
{
    public abstract class Repository
    {
        protected MathBattlesDbContext _context;
        public Repository(MathBattlesDbContext context)
        {
            _context = context;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
