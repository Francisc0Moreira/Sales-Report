using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIOLoadDataService.Models;
using System.IO;
using Microsoft.AspNetCore.Http;



namespace SIOLoadDataService
{
    internal class LoadData
    {

        private readonly OnContext context;

        private ReadData readData;

        private LoadDimensions dimensions;

        private FactTables factTables;

        private IFormFile file;

        public LoadData(OnContext context) 
        {
            this.context = context;
            readData = new ReadData(context);
            dimensions = new LoadDimensions(context);
            factTables = new FactTables(context);
        }


        public void LoadAllData(IFormFile file)
        {
            try
            {
                this.file = file;
                this.readData.setSaftData(file);

                //Load Model Data
                LoadCompany();
                LoadProducts();
                LoadClients();
                readData.ReadSAFTInvoices();

                //Load Dimensions
                dimensions.LoadDimensionsInfo();

                //Load FactTable
                factTables.InsertInFactTables();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void LoadCompany()
        {


            try
            {

                Company company = readData.ReadSAFTCompany();

                if (company == null) throw new ArgumentNullException(nameof(company));

                if (!context.Company.Any(c => c.CompanyId == company.CompanyId))
                {
                    context.Company.Add(company);
                }

                context.SaveChanges();

                Console.WriteLine($"Empresa, {company.Name}, adicionada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }


        public void LoadProducts()
        {


            try
            {

                List<Products> products = readData.ReadSAFTProducts();

                if (products == null) throw new ArgumentNullException(nameof(products));

                foreach (Products product in products)
                {
                    if (!context.Products.Any(p => p.ProductId == product.ProductId))
                    {
                        context.Products.Add(product);
                    }
                }

                context.SaveChanges();

                Console.WriteLine($"Produtos, {products.Count}, adicionados com sucesso!");


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

       


        public void LoadClients()
        {


            try
            {

                List<Clients> clients = readData.ReadSAFTClients();

                if (clients == null) throw new ArgumentNullException(nameof(clients));

                foreach (Clients client in clients)
                {
                    if (!context.Clients.Any(c => c.ClientId == client.ClientId))
                    {
                        context.Clients.Add(client);
                    }
                }

                context.SaveChanges();

                Console.WriteLine($"Clientes, {clients.Count}, adicionados com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void LoadRegion(Local local)
        {
            try
            {
                if (local == null) throw new ArgumentNullException(nameof(local));
                if (!context.Locals.Any(l => l.LocalId == local.LocalId))
                {
                    context.Locals.Add(local);
                    //Console.WriteLine($"Local adicionado com sucesso: {local.LocalId}, {local.City}");
                }

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void LoadSales(Sales sales)
        {
            try
            {

                if (sales == null) throw new ArgumentNullException(nameof(sales));
                if (!context.Sales.Any(s => s.SalesId == sales.SalesId))
                {
                    context.Sales.Add(sales);
                    Console.WriteLine($"Fatura adicionada com sucesso: {sales.SalesId}");
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void LoadSaleLine(SalesLines lines)
        {
            try
            {

                if (lines == null) throw new ArgumentNullException(nameof(lines));
                if (!context.SalesLines.Any(l => l.Line == lines.Line && l.SalesId == lines.SalesId))
                {
                    context.SalesLines.Add(lines);
                    //Console.WriteLine($"Linha de venda adicionada com sucesso: {lines.Line} da venda {lines.SalesId}");
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public void CleanDatabase()
        {
            context.Database.ExecuteSqlRaw(
                "TRUNCATE TABLE SalesLines " +
                "Delete From Sales " +
                "Delete From Clients " +
                "Delete From Locals " +
                "Delete From Products " +
                "Truncate table Company");
        }

    }
}
