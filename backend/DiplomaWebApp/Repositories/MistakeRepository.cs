using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;

namespace DiplomaWebApp.Repositories
{
    public class MistakeRepository : Repository
    {
        public MistakeRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public void AddRangeResult(List<Mistake> mistake)
        {
            _context.Mistakes.AddRange(mistake);
        }
    }
}
