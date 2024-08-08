using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class Company  
    {

        [Key]
        [Required]
        public string CompanyId { get; set; }

        public string Name { get; set; }

        public string? Nif { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? PostalCode { get; set; }

        public string? Street { get; set; }

        public string? FiscalYear { get; set; }


    }
}
