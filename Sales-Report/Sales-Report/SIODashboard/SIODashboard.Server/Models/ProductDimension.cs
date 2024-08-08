using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class ProductDimension
    {

        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string ProductKey { get; set; }

        public string? Family { get; set; }

        public string? Id { get; set; }

    }
}
