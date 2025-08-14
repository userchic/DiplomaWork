using DiplomaWebApp.Abstractions;
using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWebApp.Repositories
{
    public class GameRepository : Repository, IGameRepository
    {
        public GameRepository(MathBattlesDbContext context) : base(context)
        {
            
        }
        public ICollection<Game> GetGames()
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
