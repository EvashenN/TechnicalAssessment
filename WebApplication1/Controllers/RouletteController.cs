using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Dto;
using WebApplication1.Logging;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteController : ControllerBase
    {

        private readonly IRouletteRepository _rouletteRepo;
        private readonly ILoggerManager _logger;

        public RouletteController(IRouletteRepository rouletteRepo, ILoggerManager logger)
        {
            _rouletteRepo = rouletteRepo;
            _logger = logger;

        }

        [HttpGet]
        [Route("bet")]
        public async Task<IActionResult> Bets()
        {
            _logger.LogInfo("Fetch active bets");

            var bets = await _rouletteRepo.ActiveBets();

            _logger.LogInfo("Returned active bets");

            return Ok(bets);

        }


        [HttpGet("bet/{Id}")]
        public async Task<IActionResult> GetBet(int Id)
        {
            _logger.LogInfo($"Fetch bet by id: {Id}");

            var bet = await _rouletteRepo.BetById(Id);
            if (bet == null)
            {
                return NotFound();

            }
            _logger.LogInfo("Returned bet");

            return Ok(bet);
        }




        [HttpPost]
        [Route("bet")]
        public async Task<IActionResult> Bet(BetDto bet)
        {
            _logger.LogInfo("Creating bet");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = await _rouletteRepo.PlaceBet(bet);

            _logger.LogInfo("Complete create bet");

            return CreatedAtAction("GetBet", new { Id = res.Id }, res);

        }

        [HttpGet]
        [Route("spin")]
        public async Task<IActionResult> Spin()
        {
            _logger.LogInfo("Spin roulette");

            var res = await _rouletteRepo.Spin();
            _logger.LogInfo("Completed spin");

            return Ok(res);
        }

        [HttpGet]
        [Route("payout")]
        public async Task<IActionResult> Payout()
        {
            _logger.LogInfo("Calculate payout");

            var res = await _rouletteRepo.Payout();
            _logger.LogInfo("Completed payout");

            return Ok(res);
        }

        [HttpGet]
        [Route("previousSpins")]
        public async Task<IActionResult> ShowPreviousSpins()
        {
            _logger.LogInfo("Fetch previous spins");

            var res = await _rouletteRepo.ShowPreviousSpins();
            _logger.LogInfo("Returned previous spins");

            return Ok(res);
        }


    }
}
