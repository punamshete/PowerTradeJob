using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerTradeJob.Models
{
    public class TradePeriod
    {
        public TradePeriod(int period, double volume)
        {
            Period = period;
            Volume = volume;
        }

        /// <summary>
        /// Gets trade position
        /// </summary>
        public int Period { get; }

        /// <summary>
        /// Gets the trading volume
        /// </summary>
        public double Volume { get; }

        public override string ToString()
        {
            return $"Period={this.Period}   Volume={this.Volume}";
        }
    }
    public class PowerGrade{
        public string Period { get; set; }
        public double Volume { get; set; }
    }
}
