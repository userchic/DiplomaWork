using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class GameRepository : AbstractRepository, IGameRepository
    {
        public Game GetGame(int id)
        {
            return _context.Games.Find(id);
        }

        public ICollection<Game> GetGames()
        {
            return _context.Games.Select(x=>x).ToArray();
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
