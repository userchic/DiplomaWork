using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class RoundResultRepository : Repository
    {
        public RoundResultRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public void AddResult(RoundResults result)
        {
            _context.RoundResults.Add(result);
        }
    }
}
