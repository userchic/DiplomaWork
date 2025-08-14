using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using DiplomaWebApp.Repositories;
using DiplomaWebApp.Abstractions;

namespace DiplomaWebApp.Repositories
{
    public class RoundRepository : Repository, IRoundRepository
    {
        public RoundRepository(MathBattlesDbContext context) : base(context)
        {
        }

        public void AddRound(Round newRound)
        {
            _context.Rounds.Add(newRound);
        }
    }
}
