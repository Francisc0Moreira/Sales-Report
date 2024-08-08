using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService.Models
{
    internal class OnContext : DbContext
    {

        public DbSet<Products> Products { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<Clients> Clients { get; set; }

        public DbSet<Local> Locals { get; set; }

        public DbSet<Sales> Sales { get; set; }

        public DbSet<SalesLines> SalesLines { get; set; }

        public DbSet<TimeDimension> TimeDimension { get; set; }

        public DbSet<ProductDimension> ProductDimension { get; set; }

        public DbSet<ClientDimension> ClientDimension { get; set; }

        public DbSet<LocalDimension> LocalDimension { get; set; }

        public DbSet<SalesFactTable> SalesFactTable { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=localhost;Initial Catalog=SIO;Integrated Security=True;TrustServerCertificate=True;");
        }



    }
}
