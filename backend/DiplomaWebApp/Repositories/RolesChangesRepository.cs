using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class RolesChangesRepository:Repository
    {
        public RolesChangesRepository(MathBattlesDbContext context) : base(context)
        {
        }

        public void AddChange(RolesChange change)
        {
            _context.RolesChanges.Add(change);
        }
    }
}
