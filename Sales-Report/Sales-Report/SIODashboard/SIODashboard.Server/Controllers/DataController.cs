using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIODashboard.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;
using static SIODashboard.Server.Controllers.DataController;

namespace SIODashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        private readonly OnContext _db;

        public DataController(OnContext db) => _db = db;
        public class SalesByTime
        {

            public string S_Key { get; set; }
            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }

            public string? Month { get; set; }
            public int? Quarter { get; set; }
            public int? Semester { get; set; }


            public static string GetMonthName(int monthNumber)
            {
                switch (monthNumber)
                {
                    case 1:
                        return "January";
                    case 2:
                        return "February";
                    case 3:
                        return "March";
                    case 4:
                        return "April";
                    case 5:
                        return "May";
                    case 6:
                        return "June";
                    case 7:
                        return "July";
                    case 8:
                        return "August";
                    case 9:
                        return "September";
                    case 10:
                        return "October";
                    case 11:
                        return "November";
                    case 12:
                        return "December";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(monthNumber), "Month number must be between 1 and 12.");
                }
            }
            


        }

        [HttpGet("{year}")]
        public IActionResult GetValue(int year)
        {
            if (_db.SalesFactTable == null) return NotFound();

            var sk = _db.TimeDimension.SingleOrDefault(x => x.Year == year && x.Semester == null && x.Quarter == null && x.Month == null);

            var amount = _db.SalesFactTable.SingleOrDefault(X => X.S_Key == sk.DateKey);

            return Ok(amount);
        }

        [HttpGet("SalesByMonth")]
        public IActionResult GetSalesByMonth()
        {
            List<SalesByTime> salesByTime = new List<SalesByTime>();

            for(int i = 1; i <13; i++)
            {
                var time = _db.TimeDimension.SingleOrDefault(x => x.Month == i);
                if (time != null)
                {
                    var fact = _db.SalesFactTable.SingleOrDefault(m => m.S_Key == time.DateKey);
                    if (fact != null)
                    {
                        string mes = SalesByTime.GetMonthName((int)time.Month);
                        salesByTime.Add(new SalesByTime { S_Key = fact.S_Key, Sales_Amount = fact.Sales_Amount, Month = mes });
                    }
                }
            }
            if (salesByTime.Any())
            {
                return Ok(salesByTime);
            }
            return NotFound();
        }
        [HttpGet("SalesByQuarter")]
        public IActionResult GetSalesByQuarter()
        {
            List<TimeDimension> timeDimensions = _db.TimeDimension.Where(q => q.Quarter != null && q.Month == null).ToList();
            List<SalesByTime> salesByTimes = new List<SalesByTime>();
            foreach(TimeDimension time in timeDimensions)
            {
                SalesFactTable fact = _db.SalesFactTable.SingleOrDefault(q => q.S_Key == time.DateKey);
                salesByTimes.Add(new SalesByTime { S_Key = fact.S_Key, Sales_Amount = fact.Sales_Amount, Quarter = time.Quarter });
            }
            if (salesByTimes.Any())
            {
                return Ok(salesByTimes);
            }
            return NotFound();
        }
        [HttpGet("SalesBySemester")]
        public IActionResult GetSalesBySemester()
        {
            List<TimeDimension> timeDimensions = _db.TimeDimension.Where(s => s.Semester != null && s.Quarter == null && s.Month == null).ToList();
            List<SalesByTime> salesByTimes = new List<SalesByTime>();
            foreach (TimeDimension time in timeDimensions)
            {
                SalesFactTable fact = _db.SalesFactTable.SingleOrDefault(s => s.S_Key == time.DateKey);
                salesByTimes.Add(new SalesByTime { S_Key = fact.S_Key, Sales_Amount = fact.Sales_Amount, Semester= time.Semester });
            }
            if (salesByTimes.Any())
            {
                return Ok(salesByTimes);
            }
            return NotFound();
        }
        


    }
    

}
