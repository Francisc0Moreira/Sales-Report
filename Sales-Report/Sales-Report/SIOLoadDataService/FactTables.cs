using Microsoft.EntityFrameworkCore;
using SIOLoadDataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService
{
    internal class FactTables
    {


        private readonly OnContext context;


        public FactTables(OnContext context)
        {
            this.context = context;
        }

        public void InsertInFactTables()
        {
            try
            {
                // Limpa a FactTable
                TruncateSalesFactTable();

                //Insere os dados dos clientes na FactTable


                //Insere os dados dos Produtos na FactTable
                QuestionProductYearIds();
                QuestionProductYearFamily();

                //Insere dados relativos ao tempo na FactTable
                QuestionTimeMonthSales();
                QuestionTimeQuarterSales();
                QuestionTimeSemesterSales();
                QuestionTimeYear();

                QuestionLocalCountryYear();
                QuestionLocalCityYear();
                QuestionLocalCitySemester();
                QuestionLocalCityQuarter();
                QuestionLocalCityMonth();
                QuestionClient();
                QuestionClientMonth();
                QuestionProduct();
                QuestionProductCity();
                QuestionProductMonth();

                Console.WriteLine("Dados na FactTable inseridos");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void TruncateSalesFactTable()
        {
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE SalesFactTable");
        }



        public void QuestionProductYearFamily()
        {
            var salesData = from s in context.Sales
                            join sl in context.SalesLines on s.SalesId equals sl.SalesId
                            join p in context.Products on sl.ProductsId equals p.ProductId
                            join pd in context.ProductDimension on p.ProductId equals pd.Id
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where td.Month == null && td.Quarter == null && td.Semester == null && pd.Id == null
                            group new { td.DateKey, pd.ProductKey, sl.Total, sl.Quantity } by new { td.DateKey, pd.ProductKey } into g
                            select new
                            {
                                DateKey = g.Key.DateKey,
                                ProductKey = g.Key.ProductKey,
                                Sales_Amount = g.Sum(x => x.Total),
                                QuantitySold = g.Sum(x => x.Quantity)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.ProductKey + "." + data.DateKey,
                    Sales_Amount = data.Sales_Amount,
                    QuantitySold = data.QuantitySold
                });
            }

            context.SaveChanges();
        }

        public void QuestionProductYearIds()
        {
            var salesData = from s in context.Sales
                            join sl in context.SalesLines on s.SalesId equals sl.SalesId
                            join p in context.Products on sl.ProductsId equals p.ProductId
                            join pd in context.ProductDimension on p.ProductId equals pd.Id
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where td.Month == null && td.Quarter == null && td.Semester == null
                            group new { td.DateKey, pd.ProductKey, sl.Total, sl.Quantity } by new { td.DateKey, pd.ProductKey } into g
                            select new
                            {
                                DateKey = g.Key.DateKey,
                                ProductKey = g.Key.ProductKey,
                                Sales_Amount = g.Sum(x => x.Total),
                                QuantitySold = g.Sum(x => x.Quantity)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.ProductKey + "." + data.DateKey,
                    Sales_Amount = data.Sales_Amount,
                    QuantitySold = data.QuantitySold
                });
            }

            context.SaveChanges();
        }

        public void QuestionTimeMonthSales()
        {
            var salesData = from s in context.Sales
                            join td in context.TimeDimension on s.Date.Value.Month equals td.Month
                            group new { td.DateKey, s.Total } by td.DateKey into g
                            select new
                            {
                                DateKey = g.Key,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.DateKey,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionTimeYear()
        {
            var salesData = from s in context.Sales
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where td.Month == null && td.Quarter == null && td.Semester == null
                            group new { td.DateKey, s.Total } by td.DateKey into g
                            select new
                            {
                                DateKey = g.Key,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.DateKey,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionTimeQuarterSales()
        {
            var salesData = from s in context.Sales
                            join td in context.TimeDimension on ((s.Date.Value.Month - 1) / 3) + 1 equals td.Quarter
                            where td.Month == null
                            group new { td.DateKey, s.Total } by td.DateKey into g
                            select new
                            {
                                DateKey = g.Key,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.DateKey,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionTimeSemesterSales()
        {
            var salesData = from s in context.Sales
                            join td in context.TimeDimension on ((s.Date.Value.Month - 1) / 6) + 1 equals td.Semester
                            where td.Month == null && td.Quarter == null
                            group new { td.DateKey, s.Total } by td.DateKey into g
                            select new
                            {
                                DateKey = g.Key,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.DateKey,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }


        public void QuestionLocalCountryYear()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.Country equals dl.Country
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where dl.City == null && td.Month == null && td.Quarter == null && td.Semester == null
                            group new { dl.LocalKey, td.DateKey, s.Total } by new { dl.LocalKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.LocalKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionLocalCityYear()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.City equals dl.City
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where td.Month == null && td.Quarter == null && td.Semester == null
                            group new { dl.LocalKey, td.DateKey, s.Total } by new { dl.LocalKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.LocalKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionLocalCitySemester()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.City equals dl.City
                            join td in context.TimeDimension on (((s.Date.Value.Month - 1) / 6) + 1) equals td.Semester
                            where td.Month == null && td.Quarter == null
                            group new { dl.LocalKey, td.DateKey, s.Total } by new { dl.LocalKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.LocalKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionLocalCityQuarter()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.City equals dl.City
                            join td in context.TimeDimension on (((s.Date.Value.Month - 1) / 3) + 1) equals td.Quarter
                            where td.Month == null
                            group new { dl.LocalKey, td.DateKey, s.Total } by new { dl.LocalKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.LocalKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }
       

        public void QuestionLocalCityMonth()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.City equals dl.City
                            join td in context.TimeDimension on s.Date.Value.Month equals td.Month
                            group new { dl.LocalKey, td.DateKey, s.Total } by new { dl.LocalKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.LocalKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionClient()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join c in context.ClientDimension on cl.ClientId equals c.Id
                            join td in context.TimeDimension on s.Date.Value.Year equals td.Year
                            where td.Month == null && td.Quarter == null && td.Semester == null
                            group new { c.ClientKey, td.DateKey, s.Total } by new { c.ClientKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.ClientKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }


        public void QuestionClientMonth()
        {
            var salesData = from s in context.Sales
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join c in context.ClientDimension on cl.ClientId equals c.Id
                            join td in context.TimeDimension on s.Date.Value.Month equals td.Month
                            group new { c.ClientKey, td.DateKey, s.Total } by new { c.ClientKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.ClientKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount
                });
            }

            context.SaveChanges();
        }

        public void QuestionProduct()
        {
            var salesData = from s in context.Sales
                            join sl in context.SalesLines on s.SalesId equals sl.SalesId
                            join p in context.Products on sl.ProductsId equals p.ProductId
                            join pd in context.ProductDimension on p.ProductId equals pd.Id
                            group new { pd.ProductKey, sl.Total, sl.Quantity } by pd.ProductKey into g
                            select new
                            {
                                S_Key = g.Key,
                                Sales_Amount = g.Sum(x => x.Total),
                                QuantitySold = g.Sum(x => x.Quantity)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount,
                    QuantitySold = data.QuantitySold
                });
            }

            context.SaveChanges();
        }

        public void QuestionProductCity()
        {
            var salesData = from s in context.Sales
                            join sl in context.SalesLines on s.SalesId equals sl.SalesId
                            join p in context.Products on sl.ProductsId equals p.ProductId
                            join pd in context.ProductDimension on p.ProductId equals pd.Id
                            join cl in context.Clients on s.ClientsId equals cl.ClientId
                            join l in context.Locals on cl.LocalId equals l.LocalId
                            join dl in context.LocalDimension on l.City equals dl.City
                            group new { pd.ProductKey, dl.LocalKey, sl.Total, sl.Quantity } by new { pd.ProductKey, dl.LocalKey } into g
                            select new
                            {
                                S_Key = g.Key.ProductKey + "." + g.Key.LocalKey,
                                Sales_Amount = g.Sum(x => x.Total),
                                QuantitySold = g.Sum(x => x.Quantity)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount,
                    QuantitySold = data.QuantitySold
                });
            }

            context.SaveChanges();
        }

        public void QuestionProductMonth()
        {
            var salesData = from s in context.Sales
                            join sl in context.SalesLines on s.SalesId equals sl.SalesId
                            join p in context.Products on sl.ProductsId equals p.ProductId
                            join pd in context.ProductDimension on p.ProductId equals pd.Id
                            join td in context.TimeDimension on s.Date.Value.Month equals td.Month
                            group new { pd.ProductKey, td.DateKey, sl.Total, sl.Quantity } by new { pd.ProductKey, td.DateKey } into g
                            select new
                            {
                                S_Key = g.Key.ProductKey + "." + g.Key.DateKey,
                                Sales_Amount = g.Sum(x => x.Total),
                                QuantitySold = g.Sum(x => x.Quantity)
                            };

            foreach (var data in salesData)
            {
                context.SalesFactTable.Add(new SalesFactTable
                {
                    S_Key = data.S_Key,
                    Sales_Amount = data.Sales_Amount,
                    QuantitySold = data.QuantitySold
                });
            }

            context.SaveChanges();
        }


    }
}
