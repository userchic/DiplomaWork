using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWebApp.DataBase
{
    public class MathBattlesDbContext:DbContext
    {
        public MathBattlesDbContext(DbContextOptions<MathBattlesDbContext> options)
: base(options)
        {
            //DropCreateDatabase();
            /*bool isCreated =*/ Database.EnsureCreated();
        }

        private void DropCreateDatabase()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            Jures.AddRange(new Jure[]{
                new Jure() { Name="Руслан",Surname="Габзалилов",Fatname="Маратович",Login="GabRus",Password="1234"},
                new Jure(){ Name="Николас",Surname="Вольфгангович",Fatname="Amodeus",Login="NWA",Password="1234"},
            });
            Students.AddRange(new Student[] {
                new Student(){Name="Никита", Surname="Рудик",Fatname="Витальевич", Email=""},
                new Student(){Name="Вася", Surname="Васнин",Fatname="Иннокентиевич",Email=""},
                new Student(){Name="Вова", Surname="Авова",Fatname="Моцартович",Email=""},
                new Student(){Name="Джо", Surname="Вигин",Fatname="Александрович",Email=""},
                new Student(){Name="Хелена", Surname="Осиповна",Fatname="Николасовна",Email=""}
            });
            Tasks.AddRange(new Problem[]
            {
                new Problem(){ Text="1+2=" },
                new Problem(){ Text="1+3=" },
                new Problem(){ Text="1+4=" },
                new Problem(){ Text="1+5=" },
                new Problem(){ Text="1+6=" },
            });
            SaveChanges();
        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Jure> Jures { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Problem> Tasks { get; set; }
        public DbSet<Game> Games { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Jure>()
                .HasMany(x => x.Games)
                .WithMany(x=>x.Assessors);

            builder.Entity<Problem>()
                .HasMany(x => x.Games)
                .WithMany(x => x.Tasks);

            builder.Entity<Game>()
                .HasOne(x => x.Team1);
            builder.Entity<Game>()
                .HasOne(x => x.Team2);

            builder.Entity<Team>()
                .HasMany(x => x.Students)
                .WithMany(x=>x.Teams);
            builder.Entity<Team>().
                HasOne(x => x.ViceCaptain);
            builder.Entity<Team>().
                HasOne(x => x.Captain);
        }
    }
}
