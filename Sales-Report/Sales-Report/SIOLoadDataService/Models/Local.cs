﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIOLoadDataService.Models
{
    internal class Local
    {
        [Key]
        [Required]
        public string LocalId { get; set; }


        public string? City { get; set; }

        public string? Country { get; set; }

        public string? PostalCode { get; set; }

        public string? Street { get; set; }
    }
}
