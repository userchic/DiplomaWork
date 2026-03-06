using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWebApp.Repositories
{
    public class GameRepository : Repository, IGameRepository
    {
        int pageSize = 10;
        public GameRepository(MathBattlesDbContext context) : base(context)
        {
            
        }
        public ICollection<Game> GetGames(int page = 1)
        {
            int lastGameId = _context.Games.OrderBy(x=>x.Id).Last().Id;
            return _context.Games.OrderBy(x => -x.Id).Where(x => (x.Id >= lastGameId - pageSize * page && x.Id <= lastGameId - pageSize * (page-1))).
                Include(x => x.Team1).ThenInclude(x => x.Students).
                Include(x => x.Team2).ThenInclude(x => x.Students).
                Include(x => x.Tasks).
                Include(x => x.Assessor).
                Include(x => x.CaptainsRound).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.Breaks).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.Changes).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.RolesChange).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.RoundResults).ThenInclude(x => x.Mistakes).
                Take(10).
                ToArray();
        }
        public Game? GetGame(int id)
        {
            return _context.Games.Select(x => x).
                Include(x => x.Team1).ThenInclude(x => x.Students).
                Include(x => x.Team2).ThenInclude(x => x.Students).
                Include(x => x.Tasks).
                Include(x => x.Assessor).
                Include(x => x.CaptainsRound).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.Breaks).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.Changes).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.RolesChange).
                Include(x => x.Challenges).ThenInclude(x => x.Round).ThenInclude(x => x.RoundResults).ThenInclude(x => x.Mistakes).
                FirstOrDefault(x => x.Id == id);
        }
        public void AddGame(Game game)
        {
            _context.Games.Add(game);
        }
        public void RemoveGame(Game game)
        {
            _context.Games.Remove(game);
        }
        public void UpdateGame(Game game)
        {
            _context.Games.Update(game);
        }


    }
}
