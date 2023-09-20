using PowerTradeJob.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradeJob.Services
{
    /// <summary>
    /// Interface to get power grade data and generate report
    /// </summary>
    public interface IPowerTradeService
    {
        IEnumerable<Trade> GetTrades(DateTime date);

        void GenerateReport(DateTime LocalTime, IEnumerable<Trade> trades);
    }
}
