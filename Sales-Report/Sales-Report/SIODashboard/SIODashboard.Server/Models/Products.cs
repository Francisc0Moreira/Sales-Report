using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class Products
    {
        [Key]
        [Required]
        public string ProductId { get; set; }

        public string? Description { get; set; }

        [Column(TypeName ="decimal(10,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price_Iva { get; set; }

        public string? Family {  get; set; }

        public string? Unity { get; set; }



    }
}
