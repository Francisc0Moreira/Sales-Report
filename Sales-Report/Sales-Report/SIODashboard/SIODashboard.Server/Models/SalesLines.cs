using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class SalesLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int SalesLinesId { get; set; }

        
        public Sales? Sales {get; set; }
        
        public string? SalesId { get; set; }

        public string Line { get; set; }

        public Products? Products { get; set; }
        public string? ProductsId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName ="decimal(10,2)")]
        public decimal? ProductPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Total { get; set; }



    }
}
