using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    public class OnContext : DbContext
    {

        internal DbSet<Products> Products { get; set; }

        internal DbSet<Company> Company { get; set; }

        internal DbSet<Clients> Clients { get; set; }

        internal DbSet<Local> Locals { get; set; }

        internal DbSet<Sales> Sales { get; set; }

        internal DbSet<SalesLines> SalesLines { get; set; }

        internal DbSet<TimeDimension> TimeDimension { get; set; }

        internal DbSet<ProductDimension> ProductDimension { get; set; }

        internal DbSet<ClientDimension> ClientDimension { get; set; }

        internal DbSet<LocalDimension> LocalDimension { get; set; }

        internal DbSet<SalesFactTable> SalesFactTable { get; set; }


        public OnContext(DbContextOptions<OnContext> options) : base(options)
        {



        }
    }
}
