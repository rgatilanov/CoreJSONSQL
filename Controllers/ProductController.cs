
namespace UserApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using UserApi.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        //[Authorize]
        public ActionResult<List<Product>> GetProducts()
        {
            List<Product> products = new List<Product>();
            products.Add(new Models.Product()
            {
                Description = "echo dot(3 gen)",
                Id = Guid.NewGuid(),
                Price = 800,
            });

            products.Add(new Models.Product()
            {
                Description = "echo plus(2 gen)",
                Id = Guid.NewGuid(),
                Price = 3000,
            });

            return products;
        }
    }
}