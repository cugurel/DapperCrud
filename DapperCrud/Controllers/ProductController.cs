using Dapper;
using DapperCrud.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace DapperCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration _config;
        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("NortwindConn"));
            var products = await connection.QueryAsync<Product>("Select * from Products");
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<List<Product>>> GetAllProduct(int productId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("NortwindConn"));
            var products = await connection.QueryFirstAsync<Product>("Select * from Products where ProductID = @Id",
                new {Id = productId});
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<List<Product>>> CreateProduct(Product product)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("NortwindConn"));
            await connection.ExecuteAsync("insert into Products(ProductName, QuantityPerUnit, UnitPrice) values (@ProductName, QuantityPerUnit, UnitPrice)",product);
            return Ok();
        }
    }
}
