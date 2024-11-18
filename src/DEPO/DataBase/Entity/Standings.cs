using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFFA.src.DEPO.DataBase.Entity
{
    public class Standing
    {
        public int StandingId { get; set; }
        public int ClubId { get; set; }
        public int MatchesPlayed { get; set; }
        public int Points { get; set; }

        public Club Club { get; set; }
    }
}
