using DiplomaWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DiplomaWebApp.DataBase
{
    public class MathBattlesDbContext : DbContext
    {
        public MathBattlesDbContext(DbContextOptions<MathBattlesDbContext> options)
: base(options)
        {
            //Database.EnsureDeleted();
            DropCreateDatabase();
            /*bool isCreated =*/ //Database.EnsureCreated();
        }

        private void DropCreateDatabase()
        {
            if (Database.EnsureCreated())
            {
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
                Roles.AddRange(new Role[] {
                new Role()
                {
                    Name="Оппонент"
                },
                new Role()
                {
                    Name="Спикер"
                }
                });
                SaveChanges();
            }
        }
        public DbSet<Break> Breaks { get; set; }
        public DbSet<CaptainsRound> CaptainsRounds { get; set; }
        public DbSet<Change> Changes { get; set; }
        public DbSet<Mistake> Mistakes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolesChange> RolesChanges { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Round> Rounds { get; set; }
        public DbSet<RoundResults> RoundResults { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Jure> Jures { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Problem> Tasks { get; set; }
        public DbSet<Game> Games { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Jure>()
                .HasMany(x => x.Games)
                .WithOne(x => x.Assessor);

            builder.Entity<Game>()
                .HasMany(x => x.Tasks)
                .WithMany(x=>x.Games);

            builder.Entity<Game>()
                .HasOne(x => x.Team1);
            builder.Entity<Game>()
                .HasOne(x => x.Team2);

            builder.Entity<CaptainsRound>()
                .HasOne(x => x.Participant1);
            builder.Entity<CaptainsRound>()
                .HasOne(x => x.Participant2);

            builder.Entity<Team>()
                .HasMany(x => x.Students).
                WithMany(x=>x.Teams);
            builder.Entity<Team>().
                HasOne(x => x.ViceCaptain);
            builder.Entity<Team>().
                HasOne(x => x.Captain);

            builder.Entity<Student>().
                HasMany(x => x.OpponentRounds).
                WithOne(x => x.Opponent);
            builder.Entity<Student>().
                HasMany(x => x.SpeakerRounds).
                WithOne(x => x.Speaker);
            builder.Entity<Break>().
                HasOne(x => x.InitiatorTeam).
                WithMany(x=>x.Breaks);
        }
    }
}
