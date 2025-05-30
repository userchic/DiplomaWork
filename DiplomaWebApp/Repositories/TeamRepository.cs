using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class TeamRepository: Repository,ITeamRepository
    {
        public TeamRepository(MathBattlesDbContext context) : base(context)
        {
        }

        public Team GetTeam(int id)
        {
            return _context.Teams.FirstOrDefault(x => x.Id == id);
        }
        public void AddTeam(Team team)
        {
            _context.Add(team);
        }
    }
}
