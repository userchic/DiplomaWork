using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiplomaWebApp.Models
{
    public class CaptainsRound
    {
        [Key]
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int? Participant1Id { get; set; }
        [ForeignKey("Participant1Id")]
        public Student Participant1 { get; set; }
        public int? Participant2Id { get; set; }
        [ForeignKey("Participant2Id")]
        public Student Participant2 { get; set; }
        public int WinnerId { get; set; }
    }
}
