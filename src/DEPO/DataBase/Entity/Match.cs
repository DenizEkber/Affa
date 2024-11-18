namespace AFFA.src.DEPO.DataBase.Entity
{
    public class Match
    {
        public int MatchId { get; set; }
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public DateTime MatchDate { get; set; }

        public virtual MatchResult MatchResult { get; set; }

        public virtual Club HomeClub { get; set; }
        public virtual Club AwayClub { get; set; }
    }
}
