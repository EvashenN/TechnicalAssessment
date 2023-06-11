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

            return StatusCode(201, res);

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
