using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIODashboard.Server.DTOs;
using SIODashboard.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIODashboard.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {


        private readonly OnContext _db;

        public ProductsController(OnContext db) => _db = db;

        public class ProductCity
        {
            public string Id { get; set; }
            public string City { get; set; }

            [Column(TypeName = "decimal(10,2)")]
            public decimal? Sales_Amount { get; set; }

            public int? QuantitySold { get; set; }

        }


        [HttpGet("productCity")] // Nome do endpoint
        public IActionResult GetProductByCity()
        {
            if (_db.SalesFactTable == null) return NotFound(); // verifica se existe algum dado na tabela dos factos

            List<ProductDimension> products = _db.ProductDimension.Where(p=> p.Id != null).ToList(); // Vai buscar os produtos
            List<LocalDimension> locals = _db.LocalDimension.Where(l => l.City != null).ToList(); // Vai buscar as cidades
            List<ProductCity> productsCity = new List<ProductCity>(); // Lista onde vais armazenar a informação para a pergunta

            foreach (ProductDimension product in products) 
            {
                foreach (LocalDimension local in locals)
                {
                    SalesFactTable fact = _db.SalesFactTable.SingleOrDefault(x => x.S_Key == product.ProductKey + "." + local.LocalKey); // Vai procurar um facto na tabela com a junção das keys
                    if(fact != null) // Se existir adiciona à lista productsCity
                    {
                        productsCity.Add(new ProductCity { Id = product.Id, City = local.City, Sales_Amount = fact.Sales_Amount, QuantitySold = fact.QuantitySold });
                    }
                }
            
            }

            if(productsCity != null)
            {
                return Ok(productsCity); //retorna a lista + 200
            }
            return BadRequest(); // retorna badrequest 500
        }

    }
}
