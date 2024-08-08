using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class Sales
    {

        [Key]
        [Required]
        public string SalesId { get; set; }


        public Clients? Clients { get; set; }
        public string? ClientsId { get; set; }

        [Column(TypeName ="datetime2(7)")]
        public DateTime? Date { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Total { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? GrossTotal { get; set; }

    }
}
