using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DiplomaWebApp.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int? CaptainId { get; set; }
        public int? ViceCaptainId { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }

        [ForeignKey("CaptainId")]
        public Student Captain { get; set; }

        [ForeignKey("ViceCaptainId")]
        public Student ViceCaptain { get; set; }
    }
}