using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class RouletteService : IRouletteService
    {
        public decimal CalculateBetPayout(string betType, decimal betAmount)
        {
            decimal Amount = 0;

            if (betType.Equals("black") || betType.Equals("red") || betType.Equals("high") ||
                betType.Equals("low") || betType.Equals("even") || betType.Equals("odd"))
            {
                Amount = betAmount * 1;
            }
            else if (betType.Equals("dozen1") || betType.Equals("dozen2") || betType.Equals("dozen3") ||
                betType.Equals("column1") || betType.Equals("column2") || betType.Equals("column3"))
            {
                Amount = betAmount * 2;
            }
            else if (betType.Equals("straight"))
            {
                Amount = betAmount * 35;
            }

            return Amount;
        }

        public bool DetermineBetOutcome(string betType, int betNumber, int winningNumber)
        {
            int[] redBets = {
                 1, 3, 5, 7, 9, 12,
                 14, 16, 18, 19, 21, 23,
                 25, 27, 30, 32, 34, 36};

            int[] blackBets = {
                 2, 4, 6, 8, 10, 11,
                 13, 15, 17, 20, 22, 24,
                 26, 28, 29, 31, 33, 35 };


            int[] dozenOne = {
                 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            int[] dozenTwo = {
                 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24};

            int[] dozenThree = {
                 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36};

            int[] columnOne = {
                 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34};

            int[] columnTwo = {
                2, 5, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35};

            int[] columnThree = {
                3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36};


            bool Winner = false;

            switch (betType)
            {
                case "straight":
                    if (betNumber == winningNumber)
                        Winner = true;
                    break;

                case "odd":
                    if (winningNumber % 2 != 0)
                        Winner = true;
                    break;

                case "even":
                    if (winningNumber % 2 == 0)
                        Winner = true;
                    break;

                case "low":
                    if (winningNumber >= 1 && winningNumber <= 18)
                        Winner = true;
                    break;

                case "high":
                    if (winningNumber >= 19 && winningNumber <= 36)
                        Winner = true;
                    break;

                case "red":
                    if (redBets.Contains(winningNumber))
                        Winner = true;
                    break;

                case "black":
                    if (blackBets.Contains(winningNumber))
                        Winner = true;
                    break;

                case "dozen1":
                    if (dozenOne.Contains(winningNumber))
                        Winner = true;
                    break;

                case "dozen2":
                    if (dozenTwo.Contains(winningNumber))
                        Winner = true;
                    break;

                case "dozen3":
                    if (dozenThree.Contains(winningNumber))
                        Winner = true;
                    break;

                case "column1":
                    if (columnOne.Contains(winningNumber))
                        Winner = true;
                    break;

                case "column2":
                    if (columnTwo.Contains(winningNumber))
                        Winner = true;
                    break;

                case "column3":
                    if (columnThree.Contains(winningNumber))
                        Winner = true;
                    break;

                default:
                    Winner = false;
                    break;
            }

            return Winner;
        }

        //spin roulette
        public int GenerateNumber()
        {
            Random random = new();
            int winningNumber = random.Next(0, 36);

            return winningNumber;
        }
    }
}
