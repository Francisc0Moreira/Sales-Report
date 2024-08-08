using Microsoft.EntityFrameworkCore;
using SIOLoadDataService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService
{
    internal class LoadDimensions
    {

        private readonly OnContext context;

        public LoadDimensions(OnContext context)
        {
            this.context = context;
        }


        public void LoadDimensionsInfo()
        {
            try
            {
                TruncateDim();
                //Load Dimensions
                LoadProductsDimension(context.Products.OrderBy(x=> x.Family).ToList());
                LoadClientsDimension(context.Clients.ToList());
                LoadLocalDimension(context.Locals.ToList());
                LoadDataDimension(context.Sales.ToList());

                Console.WriteLine("Dados nas Dimensões inseridos");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LoadProductsDimension(List<Products> products)
        {
            try
            {
                List<ProductDimension> pdId = new List<ProductDimension>();
                List<ProductDimension> pdFam = new List<ProductDimension>();
                HashSet<string> families = new HashSet<string>();
                HashSet<string> ids = new HashSet<string>();
                int count = 0;

                if (products == null) throw new ArgumentNullException(nameof(products));

                foreach (Products product in products)
                {

                    if (!families.Contains(product.Family))
                    {
                        count++;
                        pdFam.Add(new ProductDimension { ProductKey = "SK_" + "Fam" + count, Family = product.Family });
                        families.Add(product.Family);
                    }
                    if (!ids.Contains(product.ProductId))
                    {
                        pdId.Add(new ProductDimension { ProductKey = "SK_" + "Fam" + count + "_" + product.ProductId, Id = product.ProductId, Family = product.Family });
                        ids.Add(product.ProductId);
                    }                  


                }

                SaveProductDim(pdId);
                SaveProductDim(pdFam);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SaveProductDim(List<ProductDimension> pd)
        {
            try
            {
                foreach (ProductDimension product in pd)
                {
                    if (!context.ProductDimension.Any(p => p.ProductKey == product.ProductKey))
                    {
                        context.ProductDimension.Add(product);
                    }

                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void LoadClientsDimension(List<Clients> clients)
        {
            try
            {
                List<ClientDimension> clId = new List<ClientDimension>();
                HashSet<string> ids = new HashSet<string>();

                if (clients == null) throw new ArgumentNullException(nameof(clients));

                foreach (Clients client in clients)
                {
                    if (!ids.Contains(client.ClientId))
                    {
                        clId.Add(new ClientDimension { ClientKey = "SK_Cl" + client.ClientId, Id = client.ClientId });
                        ids.Add(client.ClientId);
                    }                   
                }

                SaveClientDim(clId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void SaveClientDim(List<ClientDimension> cl)
        {
            try
            {
                foreach (ClientDimension client in cl)
                {
                    if (!context.ClientDimension.Any(c => c.ClientKey == client.ClientKey))
                    {
                        context.ClientDimension.Add(client);
                    }

                    context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void LoadLocalDimension(List<Local> locals)
        {
            try
            {
                HashSet<string> cities = new HashSet<string>();
                HashSet<string> countries = new HashSet<string>();

                foreach (Local local in locals)
                {
                    if (!cities.Contains(local.City))
                    {
                        LocalDimension localDimCity = new LocalDimension { LocalKey = "SK_" + local.Country + "_" + local.City,  City = local.City , Country = local.Country };
                        SaveLocalDim(localDimCity);
                        cities.Add(local.City);
                    }

                    if (!countries.Contains(local.Country))
                    {
                        LocalDimension localDimCountry = new LocalDimension { LocalKey = "SK_" + local.Country, Country = local.Country };
                        SaveLocalDim(localDimCountry);
                        countries.Add(local.Country);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }


        public void SaveLocalDim(LocalDimension ld)
        {
            try
            {
                if (!context.LocalDimension.Any(l => l.LocalKey == ld.LocalKey))
                {

                    context.LocalDimension.Add(ld);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

         public void LoadDataDimension(List<Sales> sales)
        {
            try
            {
                int year = 2023;
                List<int> quarters = new List<int> { 1, 2, 3, 4 };
                List<int> semesters = new List<int> { 1, 2 };

                foreach (Sales sale in sales)
                {

                    DateTime date = (DateTime)sale.Date;
                    int month = date.Month;
                    int quarter = getQuarter(month);
                    int semester = getSemesterMonth(month);
                    string DateKeyMonth = "SK_"+ year + "_S" + semester + "_Q" + quarter + "_M" + month;
                    TimeDimension data = new TimeDimension { DateKey = DateKeyMonth, Month = month, Quarter = quarter, Semester = semester, Year = year };
                    SaveDataDimension(data);
                }
                foreach(int quarter in quarters)
                {
                    int semester = getSemesterQuarter(quarter);
                    string DateKey = "SK_" + year + "_S" + semester + "_Q" + quarter;
                    TimeDimension data = new TimeDimension { DateKey = DateKey, Quarter = quarter, Semester = semester, Year = year };
                    SaveDataDimension(data);
                }
                foreach (int semester in semesters)
                {
                    string DateKey = "SK_" + year + "_S" + semester;
                    TimeDimension data = new TimeDimension { DateKey = DateKey, Semester = semester, Year = year };
                    SaveDataDimension(data);
                }

                string DateKeyYear = "SK_" + year;
                TimeDimension datayear = new TimeDimension { DateKey = DateKeyYear, Year = year };
                SaveDataDimension(datayear);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void SaveDataDimension(TimeDimension data)
        {
            try
            {
                if(data == null) throw new ArgumentNullException(nameof(data));
                if(!context.TimeDimension.Any(t => t.DateKey == data.DateKey))
                {
                    context.TimeDimension.Add(data);
                }

                context.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void TruncateDim()
        {
            context.Database.ExecuteSqlRaw("" +
                "TRUNCATE TABLE LocalDimension " +
                "TRUNCATE TABLE ProductDimension "+
                "TRUNCATE TABLE ClientDimension "+
                "TRUNCATE TABLE TimeDimension ");
        }

        public int getQuarter(int month)
        {
            switch (month)
            {
                case 1: return 1;
                case 2: return 1;
                case 3: return 1;
                case 4: return 2;
                case 5: return 2;
                case 6: return 2;
                case 7: return 3;
                case 8: return 3;
                case 9: return 3;
                case 10: return 4;
                case 11: return 4;
                case 12: return 4;
                default: return 0;
            }
        }

        public int getSemesterMonth(int month)
        {
            switch (month)
            {
                case 1: return 1;
                case 2: return 1;
                case 3: return 1;
                case 4: return 1;
                case 5: return 1;
                case 6: return 1;
                case 7: return 2;
                case 8: return 2;
                case 9: return 2;
                case 10: return 2;
                case 11: return 2;
                case 12: return 2;
                default: return 0;
            }
        }

        public int getSemesterQuarter(int quarter)
        {
            switch (quarter)
            {
                case 1: return 1;
                case 2: return 1;
                case 3: return 2;
                case 4: return 2;
                default: return 0;
            }
        }

    }
}
