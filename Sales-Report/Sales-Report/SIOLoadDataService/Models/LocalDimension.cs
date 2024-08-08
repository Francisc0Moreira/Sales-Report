using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService.Models
{
    internal class LocalDimension
    {

        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string LocalKey { get; set; }


        [Column(TypeName = "nvarchar(MAX)")]
        public string? City { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Country { get; set; }




    }
}
