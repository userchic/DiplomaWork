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
        
        public DateTime? StartTime { get; set; }
        public DateTime? TaskSolvingStartTime { get; set; }
        public int SolvingTime { get; set; }
        public int? Team1Id { get; set; }
        public int? Team2Id { get; set; }
        public int AssessorPoints { get; set; } = 0;
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public string CaptainsRoundFormat { get; set; }
        public string? AssessorId { get; set; }
        public bool GameEnded { get; set; } = false;
        public int? ChallengingTeamId { get; set; }
        public bool TeamRejectedToChallenge { get; set; } = false;
        [ForeignKey("AssessorId")]
        public Jure? Assessor { get; set; }
        public CaptainsRound CaptainsRound { get; set; }
        public ICollection<Problem> Tasks { get; set; }
        public ICollection<Challenge> Challenges { get; set; }

        [ForeignKey("Team1Id")]
        public Team Team1 { get; set; }

        [ForeignKey("Team2Id")]
        public Team Team2 { get; set; }
        public bool IsSolvingHasEnded()
        {
            return TaskSolvingStartTime.HasValue && DateTime.Now > TaskSolvingStartTime.Value.Add(new TimeOnly(0, SolvingTime).ToTimeSpan()); 
        }
        internal bool HasAssessor(Jure jure)
        {
            if (Assessor is not null)
                return true;
            return false;
        }

        internal bool Ongoing()
        {
            return !GameEnded && StartTime is not null;
        }
    }
}
