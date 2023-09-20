using Castle.Core.Configuration;
using CsvHelper;
using Moq;
using PowerTradeJob.Models;
using PowerTradeJob.Services;
using System.Formats.Asn1;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace PowerTradeTestProject
{

    [TestClass]
    public class PowerTradeTest
    {
        private Mock<IPowerTradeService> _powerService;
        [TestMethod]
        public void GetTradesfromAPINotNull()
        {
            _powerService = new Mock<IPowerTradeService>();
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

        [TestMethod]
        public void ExportCsvReturnsIsExists()
        {
            _powerService = new Mock<IPowerTradeService>();
            //Arrange
            var data = new List<PowerGrade>()
            {
                new PowerGrade() { Period = "23.00", Volume = 180 },
                new PowerGrade() { Period =  "00.00",Volume = 30 },
                new PowerGrade() { Period =  "01.00",Volume = 20 }

            }.AsQueryable();
            //Get local time for London
            var LocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "GMT Standard Time");
            //Creare csv file
            string directoryPath = "C:\\\\PowerTradeReports\\\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            //string fileExtension = ".csv";
            string filename = directoryPath + "PowerPosition-" + LocalTime.ToString("yyyyMMdd_HHmm") + ".csv";
            using (var writer = new StreamWriter(directoryPath + "PowerPosition-" + LocalTime.ToString("yyyyMMdd_HHmm") + ".csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(data);
            }
          
            Assert.IsTrue(File.Exists(filename));
        }

        [TestMethod]

        public void CheckCsvFileNamingConvention()
        {
            var LocalTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "GMT Standard Time");
            string directoryPath = "C:\\\\PowerTradeReports\\\\";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            //string fileExtension = ".csv";
            string fullpath = directoryPath + "PowerPosition-" + LocalTime.ToString("yyyyMMdd_HHmm") + ".csv";
          
           string filename=  Path.GetFileName(fullpath);
           string pattern = @"^(?<ExtraText>.*)(?<Extension>.csv|.CSV)$";
            Assert.IsTrue(Regex.IsMatch(filename,pattern,RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant));
        }
    }
}