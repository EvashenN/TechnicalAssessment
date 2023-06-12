using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dto;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IRouletteRepository
    {
        public Task<IEnumerable<Bet>> ActiveBets();
        public Task<Bet> BetById(int Id);
        public Task<Bet> PlaceBet(BetDto bet);
        public Task<SpinResult> Spin();
        public Task<SpinResult> SaveSpin(SpinResult spin);
        public Task<PayoutResult> Payout();
        public Task<IEnumerable<SpinResultDto>> ShowPreviousSpins();
        public Task RemoveBets();
        public Task RemoveSpinHistory();

    }
}
