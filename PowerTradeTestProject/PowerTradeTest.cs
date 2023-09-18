using Moq;
using PowerTradeJob.Services;

namespace PowerTradeTestProject
{

    [TestClass]
    public class PowerTradeTest
    {
        //private readonly IPowerTradeService _powerService;

        //public PowerTradeTest(IPowerTradeService powerService)
        //{
        //    _powerService = powerService;
        //}

        [TestMethod]
        public void GetTradesfromAPINotNull()
        {
            var _powerService = new Mock<IPowerTradeService>();
            //Get local time for London
            var LocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "GMT Standard Time");
            var trades = _powerService.Setup(m => m.GetTrades(LocalTime));
            Assert.IsNotNull(trades);
        }
        [TestMethod]
        public void GetTradesfromAPINull()
        {
            var trades = new List<string>();
            int cntData = 0;
            Assert.AreEqual(trades.Count, cntData);
        }
        [TestMethod]
        public void DirectlyExists()
        {
            bool isFolderExists = Directory.Exists("C:\\PowerTradeReports");
            Assert.IsTrue(isFolderExists);
        }
    }
}