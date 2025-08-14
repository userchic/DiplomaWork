using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class BreakRepository : Repository
    {
        public BreakRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public void AddBreak(int gameId,Break Break)
        {
            _context.Breaks.Add(Break);
        }
    }
}
