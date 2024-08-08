using System.ComponentModel.DataAnnotations.Schema;

namespace SIODashboard.Server.DTOs
{
    public class ClientDTO
    {
        public string? Name { get; set; }

        public int? year { get; set; }

        public string? Nif { get; set; }

        public string? S_Key { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? Sales_Amount { get; set; }

    }
}
