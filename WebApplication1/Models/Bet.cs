using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Number { get; set; }
        public decimal Amount { get; set; }
        public decimal Payout { get; set; }
        public bool IsWinningBet { get; set; }

    }
}
