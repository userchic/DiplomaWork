using DiplomaWebApp.Records;
using System.ComponentModel.DataAnnotations;

namespace DiplomaWebApp.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Text { get; set; } = "";
        public ICollection<Round> Rounds { get; set; }
        public ICollection<Game> Games { get; set; }
        public Problem (string text)
        {
            Text = text;
        }
        public Problem (TaskRecord taskRecord)
        {
            Id = taskRecord.Id;
            Text=taskRecord.Text;
        }
        public Problem() { }
    }
}
