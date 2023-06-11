using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class SpinResult
    {
        public int Id { get; set; }

        public int WinNumber { get; set; }
        public decimal SpinPayout { get; set; }

        public IEnumerable<Bet> Bets { get; set; }


    }

}



