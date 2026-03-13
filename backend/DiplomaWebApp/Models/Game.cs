using DiplomaWebApp.Records;
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
        
        public DateTime? PlannedStartTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? TaskSolvingStartTime { get; set; }
        public int SolvingTime { get; set; }
        public int? Team1Id { get; set; }
        public int? Team2Id { get; set; }
        public int AssessorPoints { get; set; } = 0;
        public int Team1Points { get; set; }
        public int Team2Points { get; set; }
        public string CaptainsRoundFormat { get; set; }
        public string EventPlace { get; set; }
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
        public void ConfirmStart(Jure requestingUser)
        {
            Assessor = requestingUser;
            StartTime = DateTime.UtcNow;
        }
        public void ConfirmSolvingStart()
        {
            TaskSolvingStartTime = DateTime.UtcNow;
        }
        public void SetCaptainsRoundWinner(int winnerTeamId)
        {
            CaptainsRound round = new CaptainsRound()
            {
                GameId = Id,
                Participant1Id = Team1.CaptainId,
                Participant2Id = Team2.CaptainId,
                WinnerId = winnerTeamId
            };
            ChallengingTeamId = winnerTeamId;
            CaptainsRound = round;
        }


        public void FixateChallenge(int taskId)
        {
            Challenges.Add(new Challenge()
            {
                GameId = Id,
                DeclareTime = DateTime.UtcNow,
                RequestingTeamId = ChallengingTeamId.Value,
                TaskId = taskId,
            });
        }
        public void FixateChallengeRejection()
        {
            ChallengingTeamId = Team2Id + Team1Id - ChallengingTeamId;
            TeamRejectedToChallenge = true;
        }
        public void FixateCorrectnessCheck()
        {
            Challenges.Last().IsCheckingCorrectness = true;
        }
        public void FixateChallengeAcceptance()
        {
            Challenges.Last().IsChallengeAccepted = true;
        }
        public void StartRound(Round newRound)
        {
            Challenges.Last().Round = newRound;
        }
        public void EndRound(RoundResults newRes,EndRoundRecord record)
        {
            Team1Points += record.Team1Points;
            Team2Points += record.Team2Points;
            Challenges.Last().Round.RoundResults = newRes;

            if (newRes.Correctness && !TeamRejectedToChallenge)
            {
                ChallengingTeamId = Team2Id + Team1Id - ChallengingTeamId;
            }

            List<Mistake> newMistakes = record.Mistakes.Select(x => new Mistake(x)).ToList();
            newMistakes.ForEach(x => x.ResultsId = newRes.Id);
            Challenges.Last().Round.RoundResults.Mistakes = newMistakes;
        }
        public void EndGame()
        {
            GameEnded = true;
        }
        public void ConfirmNoSolution()
        {
            Challenges.Last().Round.NoSolution = true;
        }

        internal bool IsSolvingHasEnded()
        {
            return TaskSolvingStartTime.HasValue && DateTime.Now > TaskSolvingStartTime.Value.ToLocalTime().AddMinutes(SolvingTime); 
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
