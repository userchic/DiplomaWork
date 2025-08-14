using DiplomaWebApp.DataBase;
using DiplomaWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiplomaWebApp.Repositories
{
    public class ChangeRepository : Repository
    {
        public ChangeRepository(MathBattlesDbContext context) : base(context)
        {
        }
        public void AddChange(Change newChange)
        {
            _context.Changes.Add(newChange);
        }
    }
}
