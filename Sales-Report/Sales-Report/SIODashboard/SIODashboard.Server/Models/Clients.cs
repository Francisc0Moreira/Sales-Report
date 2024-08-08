using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIODashboard.Server.Models
{
    internal class Clients
    {
        [Key]
        [Required]
        public string ClientId { get; set; }


        public string? Name { get; set; }

        public string? Nif { get; set; }

        public Local? Local { get; set; }

        public string? LocalId { get; set; }
    }
}
