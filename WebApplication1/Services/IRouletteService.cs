using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IRouletteService
    {
        public bool DetermineBetOutcome(string betType, int betNumber, int winningNumber);
        public decimal CalculateBetPayout(string betType, decimal betAmount);
        public int GenerateNumber();

    }
}
