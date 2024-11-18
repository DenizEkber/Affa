
namespace AFFA.src.DEPO.DataBase.Entity
{
    public class MatchResult
    {
        public int MatchResultId { get; set; }
        public int MatchId { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        public Match Match { get; set; }
    }


}
