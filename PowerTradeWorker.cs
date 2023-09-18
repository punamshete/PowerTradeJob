using CsvHelper;
using CsvHelper.Configuration;
using Polly.Retry;
using PowerTradeJob.Models;
using PowerTradeJob.Services;
using System.Formats.Asn1;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PowerTradeJob
{
    /// <summary>
    /// Worker class for Power Trade position
    /// </summary>
    public class PowerTradeWorker : BackgroundService
    {
        private readonly ILogger<PowerTradeWorker> _logger;
        private readonly IPowerTradeService _powerService;
        private readonly IConfiguration _configuration;


        public PowerTradeWorker(ILogger<PowerTradeWorker> logger, IPowerTradeService powerService, IConfiguration config)
        {
            _logger = logger;
            _powerService = powerService;
            _configuration = config;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(TimeSpan.FromMinutes
                    (_configuration.GetValue<int>("AppSettings:JobDuration")), stoppingToken);

                //Get local time for London
                var LocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "GMT Standard Time");

                _logger.LogInformation("Localtime: {LocalTime}", LocalTime);

                var trades =_powerService.GetTrades(LocalTime);

                _powerService.GenerateReport(LocalTime, trades);
                _logger.LogInformation("Report generated successfully : {LocalTime}", LocalTime);

            }
        }

    }
}