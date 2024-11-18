using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFFA.src.DEPO.DataBase.Entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public decimal Balance { get; set; }
        public List<Bet> Bets { get; set; }
    }

}
