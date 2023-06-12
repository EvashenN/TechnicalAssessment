﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Dto;
using WebApplication1.Logging;
using WebApplication1.Models;
using WebApplication1.Repository;
using WebApplication1.Services;
using Xunit;

namespace web_api_tests.Controllers
{
    public class RouletteControllerTests
    {
        private readonly Mock<IRouletteRepository> _mockRepo;
        private readonly Mock<ILoggerManager> _mockLogger;
        private readonly RouletteController _controller;

        public RouletteControllerTests()
        {
            _mockRepo = new Mock<IRouletteRepository>();
            _mockLogger = new Mock<ILoggerManager>();
            _controller = new RouletteController(_mockRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetBets_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Bets();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetBets_WhenCalled_ReturnsAllItems()
        {
            //Arrange
            var bets = new List<Bet>()
            {
                new Bet() { Id=1,Type="red",Amount=10,Number=0},
                new Bet() { Id=1,Type="odd",Amount=100,Number=0},
                new Bet() { Id=1,Type="straight",Amount=25,Number=3 }
            }.AsEnumerable();

            _mockRepo.Setup(r => r.ActiveBets()).Returns(Task.FromResult(bets));

            // Act
            var okResult = await _controller.Bets() as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<Bet>>(okResult.Value);
            Assert.Equal(bets.Count(), items.Count);
        }


        [Fact]
        public async Task GetBetById_UnknownIdPassed_ReturnsNotFoundResult()
        {
            // Arrange
            Bet bResult = null;
            int SuppliedId = 100;
            _mockRepo.Setup(r => r.BetById(SuppliedId)).Returns(Task.FromResult(bResult));

            // Act
            var notFoundResult = await _controller.GetBet(SuppliedId);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
        }

        [Fact]
        public async Task GetBetById_ExistingIdPassed_ReturnsOkResult()
        {
            // Arrange
            int SuppliedId = 1;

            var response = new Bet()
            {
                Id = 4,
                Type = "straight",
                Number = 40,
                Amount = 50
            };

            _mockRepo.Setup(r => r.BetById(SuppliedId)).Returns(Task.FromResult(response));

            // Act
            var okResult = await _controller.GetBet(SuppliedId);

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task GetBetById_ExistingIdPassed_ReturnsCorrectObject()
        {
            // Arrange
            int SuppliedId = 1;

            var response = new Bet()
            {
                Id = 4,
                Type = "straight",
                Number = 40,
                Amount = 50
            };

            _mockRepo.Setup(r => r.BetById(SuppliedId)).Returns(Task.FromResult(response));

            // Act
            var okResult = await _controller.GetBet(SuppliedId) as OkObjectResult;

            // Assert
            Assert.IsType<Bet>(okResult.Value);
            Assert.Equal(response.Id, (okResult.Value as Bet).Id);
        }




        [Fact]
        public async Task PlaceBet_InvalidObjectPassed_ReturnsBadRequest()
        {
            // Arrange
            BetDto testBet = new();
            _controller.ModelState.AddModelError("Type", "Required");
            _controller.ModelState.AddModelError("Number", "Required");
            _controller.ModelState.AddModelError("Amount", "Required");

            // Act
            var badResponse = await _controller.Bet(testBet);

            // Assert
            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public async Task PlaceBet_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            var newBet = new BetDto()
            {
                Type = "straight",
                Number = 30,
                Amount = 50
            };

            var response = new Bet()
            {
                Type = "straight",
                Number = 30,
                Amount = 50
            };

            _mockRepo.Setup(r => r.PlaceBet(It.IsAny<BetDto>())).Returns(Task.FromResult(response));

            // Act        
            var createdResponse = await _controller.Bet(newBet);
            var obj = createdResponse as ObjectResult;
            var resposeObj = obj.Value as Bet;

            // Assert
            Assert.IsType<Bet>(resposeObj);
            Assert.Equal(newBet.Type, resposeObj.Type);
            Assert.Equal(newBet.Number, resposeObj.Number);
            Assert.Equal(newBet.Amount, resposeObj.Amount);

        }

        [Fact]
        public async Task PlaceBet_ValidObjectPassed_ReturnsCreated()
        {
            // Arrange
            var testBet = new BetDto()
            {
                Type = "straight",
                Number = 30,
                Amount = 50
            };

            var response = new Bet()
            {
                Id = 3,
                Type = "straight",
                Number = 30,
                Amount = 50
            };

            _mockRepo.Setup(r => r.PlaceBet(testBet)).Returns(Task.FromResult(response));

            // Act        
            var createdResponse = await _controller.Bet(testBet);

            //Assert
            Assert.IsType<CreatedAtActionResult>(createdResponse);
        }

        [Fact]
        public async Task Payout_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Payout();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }


        [Fact]
        public async Task Payout_WhenCalled_ReturnsCorrectObject()
        {
            // Arrange
            var payout = new PayoutResult()
            {
                TotalWinings = 100
            };

            _mockRepo.Setup(r => r.Payout()).Returns(Task.FromResult(payout));

            // Act
            var okResult = await _controller.Payout() as ObjectResult;

            // Assert
            Assert.IsType<PayoutResult>(okResult.Value);
        }

        [Fact]
        public async Task Spin_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.Spin();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }

        [Fact]
        public async Task Spin_WhenCalled_ReturnsCorrectObject()
        {
            // Arrange
            SpinResult spin = new()
            {
                Id = 1,
                SpinPayout = 10,
                WinNumber = 9
            };

            _mockRepo.Setup(r => r.Spin()).Returns(Task.FromResult(spin));

            // Act
            var okResult = await _controller.Spin() as ObjectResult;

            // Assert
            Assert.IsType<SpinResult>(okResult.Value);
        }

        [Fact]
        public async Task PreviousSpins_WhenCalled_ReturnsOkResult()
        {
            // Act
            var okResult = await _controller.ShowPreviousSpins();

            // Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }


        [Fact]
        public async Task PreviousSpins_WhenCalled_ReturnsAllItems()
        {
            //Arrange
            var spins = new List<SpinResultDto>()
            {
                new SpinResultDto() { SpinPayout=100,WinNumber=7 },
                new SpinResultDto() { SpinPayout=50,WinNumber=8},
                new SpinResultDto() { SpinPayout=80,WinNumber=17 }
            }.AsEnumerable();

            _mockRepo.Setup(r => r.ShowPreviousSpins()).Returns(Task.FromResult(spins));

            // Act
            var okResult = await _controller.ShowPreviousSpins() as OkObjectResult;

            // Assert
            var items = Assert.IsType<List<SpinResultDto>>(okResult.Value);
            Assert.Equal(spins.Count(), items.Count);
        }

    }
}
