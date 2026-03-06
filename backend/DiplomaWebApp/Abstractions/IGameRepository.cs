using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface IGameRepository:IRepository
    {
        public ICollection<Game> GetGames(int page=1);
        public Game? GetGame(int id);
        public void AddGame(Game game);
        public void RemoveGame(Game game);
        public void UpdateGame(Game game);
    }
}