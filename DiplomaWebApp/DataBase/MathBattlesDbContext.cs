using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;
namespace DiplomaWebApp.DataBase
{
    public class MathBattlesDbContext:DbContext
    {
        public MathBattlesDbContext(DbContextOptions<MathBattlesDbContext> options)
: base(options)
        {
            /*bool isCreated =*/ Database.EnsureCreated();
        }
        public DbSet<Jure> Jures { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Problem> Tasks { get; set; }
        public DbSet<Team> Teams { get; set; }  
        public DbSet<Game> Games { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
