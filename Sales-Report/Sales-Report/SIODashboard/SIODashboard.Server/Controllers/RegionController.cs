using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIODashboard.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIODashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly OnContext _db;

        public RegionController(OnContext db) => _db = db;

        public class LocalYear
        {
            public string City { get; set; }

            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }
        }

        public class LocalCountry
        {
            public string Country { get; set; }

            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }
        }

        public class LocalTime
        {
            public string City { get; set; }

            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }

            public int? time { get; set; }


        }


        [HttpGet("localsyears")]
        public IActionResult GetLocalsByYear()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();


                List<LocalDimension> locals = _db.LocalDimension.Where(x => x.City != null).ToList();
                List<TimeDimension> years = _db.TimeDimension.Where(x => x.Month == null && x.Quarter == null && x.Semester == null).ToList();
                List<LocalYear> localyear = new List<LocalYear>();

                foreach (LocalDimension local in locals)
                {
                    foreach (TimeDimension year in years)
                    {
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == local.LocalKey + "." + year.DateKey);

                        if (skey != null)
                        {
                            localyear.Add(new LocalYear { City = local.City, Sales_Amount = skey.Sales_Amount });
                        }
                    }
                }

                return Ok(localyear);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("countrys")]
        public IActionResult GetCountrysYear()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();

                List<LocalDimension> locals = _db.LocalDimension.Where(x => x.City == null).ToList();
                List<TimeDimension> years = _db.TimeDimension.Where(x => x.Month == null && x.Quarter == null && x.Semester == null).ToList();
                List<LocalCountry> countrysyear = new List<LocalCountry>();

                foreach (LocalDimension local in locals)
                {
                    foreach (TimeDimension year in years)
                    {
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == local.LocalKey + "." + year.DateKey);

                        if (skey != null)
                        {
                            countrysyear.Add(new LocalCountry { Country = local.Country, Sales_Amount = skey.Sales_Amount });
                        }
                    }
                }

                return Ok(countrysyear);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("localsMonth")]
        public IActionResult GetLocalsByMonth()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();

                List<LocalDimension> locals = _db.LocalDimension.Where(x => x.City != null).ToList();
                List<TimeDimension> months = _db.TimeDimension.Where(x => x.Month != null).ToList();
                List<LocalTime> localtime = new List<LocalTime>();

                foreach (LocalDimension local in locals)
                {
                    foreach (TimeDimension month in months)
                    {
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == local.LocalKey + "." + month.DateKey);

                        if (skey != null)
                        {
                            localtime.Add(new LocalTime { City = local.City, Sales_Amount = skey.Sales_Amount, time = month.Month });
                        }
                    }
                }

                return Ok(localtime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("localsQuarter")]
        public IActionResult GetLocalsByQuarter()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();

                List<LocalDimension> locals = _db.LocalDimension.Where(x => x.City != null).ToList();
                List<TimeDimension> quarters = _db.TimeDimension.Where(x => x.Quarter != null && x.Month == null).ToList();
                List<LocalTime> localtime = new List<LocalTime>();

                foreach (LocalDimension local in locals)
                {
                    foreach (TimeDimension quarter in quarters)
                    {
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == local.LocalKey + "." + quarter.DateKey);

                        if (skey != null)
                        {
                            localtime.Add(new LocalTime { City = local.City, Sales_Amount = skey.Sales_Amount, time = quarter.Quarter });
                        }
                    }
                }

                return Ok(localtime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("localsSemester")]
        public IActionResult GetLocalsBySemester()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();

                List<LocalDimension> locals = _db.LocalDimension.Where(x => x.City != null).ToList();
                List<TimeDimension> semesters = _db.TimeDimension.Where(x => x.Semester != null && x.Month == null && x.Quarter == null).ToList();
                List<LocalTime> localtime = new List<LocalTime>();

                foreach (LocalDimension local in locals)
                {
                    foreach (TimeDimension semester in semesters)
                    {
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == local.LocalKey + "." + semester.DateKey);

                        if (skey != null)
                        {
                            localtime.Add(new LocalTime { City = local.City, Sales_Amount = skey.Sales_Amount, time = semester.Semester });
                        }
                    }
                }

                return Ok(localtime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }






    }
}