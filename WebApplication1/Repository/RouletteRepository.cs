using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Context;
using WebApplication1.Dto;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Repository
{
    public class RouletteRepository : IRouletteRepository
    {
        private readonly DapperContext _context;
        private readonly IRouletteService _rouletteService;

        public RouletteRepository(DapperContext context, IRouletteService rouletteService)
        {
            _context = context;
            _rouletteService = rouletteService;
        }

        public async Task<Bet> PlaceBet(BetDto bet)
        {
            var query = "INSERT INTO Bet (Type,Number, Amount,Payout,isWinningBet) VALUES (@Type, @Number, @Amount,@Payout,@isWinningBet);" +
                        "SELECT last_insert_rowid()";

            var newBet = new Bet()
            {
                Type = bet.Type,
                Amount = bet.Amount,
                Number = bet.Number,
                Payout = _rouletteService.CalculateBetPayout(bet.Type, bet.Amount)
            };

            var parameters = new DynamicParameters();
            parameters.Add("Type", bet.Type, DbType.String);
            parameters.Add("Number", bet.Number, DbType.Int32);
            parameters.Add("Amount", bet.Amount, DbType.Decimal);
            parameters.Add("Payout", newBet.Payout, DbType.Decimal);
            parameters.Add("isWinningBet", 0, DbType.Boolean);

            //save bet details to DB
            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                newBet.Id = id;
                return newBet;
            }
        }

        public async Task<IEnumerable<SpinResultDto>> ShowPreviousSpins()
        {
            var query = "SELECT rowid AS Id,WinNumber,SpinPayout FROM Spin";

            using (var connection = _context.CreateConnection())
            {
                var storedSpins = await connection.QueryAsync<SpinResultDto>(query);

                return storedSpins;
            }
        }

        public async Task<SpinResult> Spin()
        {
            SpinResult spin = new()
            {
                WinNumber = _rouletteService.GenerateNumber()
            };

            spin.Bets = await ActiveBets();

            foreach (var bet in spin.Bets)
            {
                //check if there are winning bets based on spin result
                bet.IsWinningBet = _rouletteService.DetermineBetOutcome(bet.Type, bet.Number, spin.WinNumber);
                if (bet.IsWinningBet)
                {
                    spin.SpinPayout += bet.Payout;
                }
            }

            //save spin result to DB
            await SaveSpin(spin);
            return spin;
        }

        public async Task<PayoutResult> Payout()
        {
            PayoutResult payout = new();
            var spins = await ShowPreviousSpins();

            foreach (var sp in spins)
            {
                payout.TotalWinings += sp.SpinPayout;
            }

            // Remove active bets
            await RemoveBets();

            // Remove spins
            await RemoveSpinHistory();

            return payout;
        }

        public async Task RemoveBets()
        {
            var query = "DELETE FROM Bet";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query);
            }
        }

        public async Task<IEnumerable<Bet>> ActiveBets()
        {
            var query = "SELECT rowid AS Id,Type,Number, Amount,Payout,isWinningBet FROM Bet";
            using (var connection = _context.CreateConnection())
            {
                var storedBets = await connection.QueryAsync<Bet>(query);
                return storedBets;
            }
        }

        public async Task<SpinResult> SaveSpin(SpinResult spin)
        {
            var query = "INSERT INTO Spin (WinNumber,SpinPayout) VALUES (@WinNumber, @SpinPayout);" +
                        "SELECT last_insert_rowid()";

            var newSpin = new SpinResult()
            {
                WinNumber = spin.WinNumber,
                SpinPayout = spin.SpinPayout

            };

            var parameters = new DynamicParameters();
            parameters.Add("WinNumber", spin.WinNumber, DbType.Int32);
            parameters.Add("SpinPayout", spin.SpinPayout, DbType.Decimal);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                newSpin.Id = id;
                return newSpin;
            }
        }

        public async Task RemoveSpinHistory()
        {
            var query = "DELETE FROM Spin";

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query);
            }
        }
    }
}
