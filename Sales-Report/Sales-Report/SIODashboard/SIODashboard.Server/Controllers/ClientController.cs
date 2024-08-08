using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIODashboard.Server.Models;
using SIODashboard.Server.DTOs;
using static SIODashboard.Server.Controllers.RegionController;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIODashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly OnContext _db;

        public ClientController(OnContext db) => _db = db;

        public class ClientTime
        {
            public string name { get; set; }

            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }

            public int? time { get; set; }


        }

        [HttpGet]
        public IActionResult GetAllClientsValue()
        {
            if (_db.SalesFactTable == null) return NotFound();

            List<ClientDTO> clientSales = new List<ClientDTO>();
            List<Clients> clients = _db.Clients.ToList();

            foreach (Clients client in clients)
            {
                ClientDimension clientDim = _db.ClientDimension.Where(c => c.Id == client.ClientId).FirstOrDefault();
                TimeDimension year = _db.TimeDimension.Where(y => y.Month == null && y.Quarter == null && y.Semester == null).SingleOrDefault();
                SalesFactTable fact = _db.SalesFactTable.SingleOrDefault(x=> x.S_Key == clientDim.ClientKey + "." + year.DateKey);

                clientSales.Add(new ClientDTO { Name = client.Name, year=year.Year, Nif = client.Nif, S_Key = fact.S_Key, Sales_Amount = fact.Sales_Amount });
            }

            return Ok(clientSales);
        }


        [HttpGet("clientsMonth")]
        public IActionResult GetLocalsByMonth()
        {
            try
            {
                if (_db.SalesFactTable == null)
                    return NotFound();

                List<Clients> clients = _db.Clients.ToList();
                List<ClientTime> Clienttime = new List<ClientTime>();

                foreach (Clients client in clients)
                {
                    for(int i = 1; i<13;i++)
                    {
                        ClientDimension clientDim = _db.ClientDimension.Where(c => c.Id == client.ClientId).FirstOrDefault();
                        TimeDimension timeDim = _db.TimeDimension.Where(t=> t.Month == i).FirstOrDefault();
                        SalesFactTable skey = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == clientDim.ClientKey + "." + timeDim.DateKey);

                        if (skey != null)
                        {
                            Clienttime.Add(new ClientTime { name = client.Name, Sales_Amount = skey.Sales_Amount, time = timeDim.Month });
                        }
                    }
                }

                return Ok(Clienttime);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

       
    }
}
