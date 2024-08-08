using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using SIOLoadDataService.Models;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Http;

namespace SIOLoadDataService
{
    public class RunSaft
    {
        private readonly IServiceProvider _serviceProvider;

        public RunSaft(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static string ExecuteSaft(IFormFile file)
        {
            using OnContext context = new OnContext();

            // Aplicar migrações
            context.Database.Migrate();


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LoadData loadData = new LoadData(context);
            loadData.CleanDatabase();
            loadData.LoadAllData(file);

            return "Ficheiro carregado com sucesso";
        }
    }
}
