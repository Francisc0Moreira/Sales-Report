using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService.Models
{
    internal class SalesFactTable
    {

        [Key]
        public string S_Key { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Sales_Amount { get; set; }

        public int? QuantitySold { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Sales_Profit { get; set; }

    }
}
