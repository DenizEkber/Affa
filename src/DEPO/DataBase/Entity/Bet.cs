namespace AFFA.src.DEPO.DataBase.Entity
{
    public class Bet
    {
        public int BetId { get; set; }
        public int CustomerId { get; set; }
        public int TeamId { get; set; }
        public int MatchId { get; set; }  
        public decimal Amount { get; set; }
        public bool IsPaid { get; set; }
        public int PredictedHomeScore { get; set; }
        public int PredictedAwayScore { get; set; }

        public Customer Customer { get; set; }
        public Club Team { get; set; }
        public Match Match { get; set; }
    }
}
