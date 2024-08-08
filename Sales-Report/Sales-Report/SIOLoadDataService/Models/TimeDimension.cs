using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService.Models
{
    internal class TimeDimension
    {
        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string DateKey { get; set; }

        public Int32? Month { get; set; }
        public Int32? Quarter { get; set; }
        public Int32? Semester { get; set; }
        public Int32? Year { get; set; }
        

    }
}
