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
                new Student(){Name="Никита", Surname="Рудик",Fatname="Витальевич", Email="",EducationFacility="Школа приключений"},
                new Student(){Name="Вася", Surname="Васнин",Fatname="Иннокентиевич",Email="",EducationFacility="Школа приключений"},
                new Student(){Name="Вова", Surname="Авова",Fatname="Моцартович",Email="",EducationFacility="Школа приключений"},
                new Student(){Name="Джо", Surname="Вигин",Fatname="Александрович",Email="",EducationFacility="Школа приключений"},
                new Student(){Name="Хелена", Surname="Осиповна",Fatname="Николасовна",Email="",EducationFacility="Школа приключений"}
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
            builder.Entity<Game>()
                .HasOne(x => x.Assessor);

            builder.Entity<Game>()
                .HasMany(x => x.Tasks);

            

            builder.Entity<Game>()
                .HasOne(x => x.Team1);
            builder.Entity<Game>()
                .HasOne(x => x.Team2);

            builder.Entity<CaptainsRound>()
                .HasOne(x => x.Participant1);
            builder.Entity<CaptainsRound>()
                .HasOne(x => x.Participant2);

            builder.Entity<Game>()
                .HasOne(x => x.CaptainsRound)
                .WithOne(x=>x.Game);

            builder.Entity<Student>()
                .HasMany(x=>x.Teams)
                .WithMany(x => x.Students);
                
            builder.Entity<Team>().
                HasOne(x => x.ViceCaptain);
            builder.Entity<Team>().
                HasOne(x => x.Captain);

            builder.Entity<Round>()
                .HasOne(x => x.Opponent)
                .WithMany(x=>x.OpponentRounds).HasForeignKey(x=>x.OpponentId);
            builder.Entity<Round>()
                .HasOne(x => x.Speaker)
                .WithMany(x => x.SpeakerRounds).HasForeignKey(x => x.SpeakerId);

            builder.Entity<Break>().
                HasOne(x => x.InitiatorTeam).
                WithMany(x=>x.Breaks);
        }
    }
}
