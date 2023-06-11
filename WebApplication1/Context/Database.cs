using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Context
{
    public class Database : IDatabase
    {
        private readonly DapperContext _context;

        public Database(DapperContext context)
        {
            _context = context;
        }

        public async Task Setup()
        {
            using (var connection = _context.CreateConnection())
            {
                //check if db tables exists if not create tables
                var tables = await connection.QueryAsync<string>("SELECT name FROM sqlite_master WHERE type='table';");

                if (tables.Contains("Bet") == false)
                {
                    connection.Execute("Create Table Bet (" +
                    "Type VARCHAR(100) NOT NULL," +
                     "Number INT ," +
                     "payout DECIMAL ," +
                     "isWinningBet BOOLEAN ," +
                    "Amount DECIMAL);");
                }
                if (tables.Contains("Spin") == false)
                {
                    connection.Execute("Create Table Spin (" +
                  "SpinPayout DECIMAL ," +
                  "WinNumber INT);");
                }

            }

        }
    }
}
