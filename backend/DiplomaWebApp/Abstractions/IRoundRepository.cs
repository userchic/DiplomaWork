using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Abstractions
{
    public interface IRoundRepository:IRepository
    {
        void AddRound(Round newRound);
    }
}
