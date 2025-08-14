using DiplomaWebApp.Records;

namespace DiplomaWebApp.Models
{
    public class Mistake
    {
        public int Id { get; set; }
        public int ResultsId { get; set; }
        public int OpponentsCost { get; set; }
        public int JureCost { get; set; }
        public string Text { get; set; }
        public RoundResults Results {  get; set; }
        public Mistake()
        {

        }
        public Mistake(MistakeRecord mistakeRecord)
        {
            OpponentsCost = mistakeRecord.OpponentCost;
            JureCost = mistakeRecord.JureCost;
            Text = mistakeRecord.Text;
        }
    }
}
