using DiplomaWebApp.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaWebApp.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; } = "";
        
        public DateTime StartTime { get; set; }
        public int SolvingTime { get; set; }
        public int? Team1Id { get; set; }
        public int? Team2Id { get; set; }
        public int Team1Points { get; set; } = 0;
        public int Team2Points { get; set; } = 0;
        public int AssessorPoints { get; set; } = 0;

        public string CaptainsRoundFormat { get; set; }
        public ICollection<Problem> Tasks { get; set; }
        public ICollection<Jure> Assessors { get; set; }

        [ForeignKey("Team1Id")]
        public Team Team1 { get; set; }

        [ForeignKey("Team2Id")]
        public Team Team2 { get; set; }

        
    }
}
