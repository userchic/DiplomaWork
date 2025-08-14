using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface ITeamRepository:IRepository
    {
        public Team GetTeam(int id);
        public void AddTeam(Team team);
    }
}