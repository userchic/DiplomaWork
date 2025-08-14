using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class RoleRepository : Repository
    {
        public RoleRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public Role GetRole(int roleId)
        {
            return _context.Roles.First(x => x.Id == roleId);
        }
    }
}
