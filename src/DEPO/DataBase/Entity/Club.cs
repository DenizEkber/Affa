using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFFA.src.DEPO.DataBase.Entity
{
    public class Club
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        public ICollection<Match> HomeMatches { get; set; }
        public ICollection<Match> AwayMatches { get; set; }
        public Standing Standing { get; set; }
    }
}
