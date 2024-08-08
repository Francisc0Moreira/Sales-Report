using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIODashboard.Server.Models;

namespace SIODashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly OnContext _db;

        public CompanyController(OnContext db) => _db = db;

        [HttpGet]
        public IActionResult getCompany()
        {
            if (_db.Company == null) return NotFound();

            Company company = _db.Company.FirstOrDefault();

            return Ok(company);
        }

    }
}
