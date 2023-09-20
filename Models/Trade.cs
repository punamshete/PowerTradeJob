using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PowerTradeJob.Models
{
    public class Trade
    {
        public Trade(DateTime date, IEnumerable<TradePeriod> periods)
        {
            this.Date = date;
            this.Periods = periods.ToList();
        }

        /// <summary>
        /// The date of the trade position
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets all trade volumes 
        /// </summary>
        public List<TradePeriod> Periods { get; }

        
    }
}
