using CsvHelper;
using Microsoft.Extensions.Logging;
using PowerTradeJob.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PowerTradeJob.Services
{
    public class PowerTradeService : IPowerTradeService
    {
        PowerService _powerService = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PowerTradeWorker> _logger;

        public PowerTradeService(IConfiguration config, ILogger<PowerTradeWorker> logger)
        {
            _powerService = new PowerService();
            _configuration = config;
            _logger = logger;
        }

        /// <summary>
        /// Get power trades data from dll
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public IEnumerable<Trade> GetTrades(DateTime date)
        {
            try
            {
                var trades = (_powerService.GetTrades(date)).ToList();
                var results = new List<Trade>();
                trades.ForEach(trade =>
                {
                    var periods = trade
                    .Periods
                    .Select(tp => new TradePeriod(tp.Period, tp.Volume))
                    .ToList();
                    var tradePosition = new Trade(date, periods);
                    results.Add(tradePosition);
                });
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError("Get Trades method failed : {ex.Message}", ex.Message);
                throw ex;
            }
        }

        public void GenerateReport(DateTime LocalTime, IEnumerable<Trade> trades)
        {
            try
            {
                // calculate volume group by period
                var lstcon = trades
                       .SelectMany(s => s.Periods)
                       .GroupBy(yr => yr.Period)
                       .Select(g => new
                       {
                           Key = g.Key,
                           Value = g.Sum(s => s.Volume)
                       })
                       .ToList();

                var start = LocalTime.Date.AddHours(_configuration.GetValue<int>("AppSettings:JobStartTime"));
                List<PowerGrade> finallist = new List<PowerGrade>();

                //print 24-hour format period from list
                for (int i = 0; i < lstcon.Count; i++)
                {
                    finallist.Add(new PowerGrade() { Period = start.AddMinutes(_configuration.GetValue<int>("AppSettings:PeriodDuration") * i).ToString("HH:mm"), Volume = lstcon[i].Value });

                }

                //Creare csv file
                string directoryPath = _configuration.GetValue<string>("AppSettings:LogFilePath");
        
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                using (var writer = new StreamWriter(directoryPath + "PowerPosition-" + LocalTime.ToString("yyyyMMdd_HHmm") + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(finallist);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Generate Reports failed : {ex.Message}", ex.Message);
                throw ex;
            }

        }
    }
}
