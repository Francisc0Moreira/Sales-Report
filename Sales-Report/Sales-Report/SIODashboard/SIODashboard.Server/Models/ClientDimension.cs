using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class ClientDimension
    {

        [Key]
        [Column(TypeName = "nvarchar(50)")]
        public string ClientKey { get; set; }

        public string? Id { get; set; }

    }
}
